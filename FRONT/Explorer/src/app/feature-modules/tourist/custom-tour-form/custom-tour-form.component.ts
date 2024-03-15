import { Component } from '@angular/core';
import { Status, Tour, TourDifficulty, TransportType } from '../../tour-authoring/model/tour.model';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Keypoint } from '../../tour-authoring/model/keypoint.model';
import { RouteQuery } from 'src/app/shared/model/routeQuery.model';
import { TourAuthoringService } from '../../tour-authoring/tour-authoring.service';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { RouteInfo } from 'src/app/shared/model/routeInfo.model';

@Component({
  selector: 'xp-custom-tour-form',
  templateUrl: './custom-tour-form.component.html',
  styleUrls: ['./custom-tour-form.component.css']
})
export class CustomTourFormComponent {
  public tour: Tour;
  public tourForm: FormGroup;
  public tourId: number;
  public keypoints: Keypoint[] = [];
  public selectedKeypoint: Keypoint;
  public mode: string = 'add';
  public routeQuery: RouteQuery;
  
  public openPublicKeypointList: boolean = false;
  public publicKeypoints: Keypoint[];

  constructor(private tourAuthoringService: TourAuthoringService, private router: Router, private route: ActivatedRoute) {
    this.tour = { description: '', difficulty: TourDifficulty.EASY, status: Status.DRAFT, name: '', price: 0, transportType: TransportType.WALK, userId: 0, id:0}
    this.tourForm = new FormGroup({
      name: new FormControl('', [Validators.required]),
      description: new FormControl(''),
      difficulty: new FormControl(''),
      transportType: new FormControl(''),
    });
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe((params: ParamMap) => {
      this.tourId = Number(params.get('id'));

      if(this.tourId !== 0){
        this.tourAuthoringService.getTourById(this.tourId).subscribe((res: Tour) => {
          this.tour = res;
          this.tourForm.patchValue(this.tour);
          
          this.getTourKeypoints();
        });

        this.tourAuthoringService.getPublicKeypoints().subscribe(res => {
          this.publicKeypoints = res.results;
        })
      }
    });
  }

  ngOnChanges(): void {
  }

  saveTour(feedback: Boolean = true): void {
    let tour: Tour = {
      id: this.tourId,
      userId: -1,
      name: this.tourForm.value.name || "",
      description: this.tourForm.value.description || "",
      price: 0,
      difficulty: this.tourForm.value.difficulty || "",
      transportType: this.tourForm.value.transportType || "",
      status: this.keypoints.length < 2 ? Status.CUSTOM : Status.DRAFT,
      tags: [],
    };

    if(this.tourId === 0){
      tour.statusUpdateTime = new Date();
      this.tourAuthoringService.addCustomTour(tour).subscribe({
        next: (newTour) => { 
          window.alert("You have successfuly saved your custom tour");
          this.router.navigate(
            ['/custom-tour', newTour.id]
          );
        }
      });
    }else{
      tour.distance = this.tour.distance;
      tour.duration = this.tour.duration;
      this.tourAuthoringService.updateTour(tour).subscribe({
        next: (updatedTour) => { 
          if(feedback)
            window.alert("You have successfuly saved your tour");
          this.tour = updatedTour;
          this.routeQuery = {
            keypoints: this.keypoints,
            transportType: this.tour.transportType
          }
        }
      });
    } 
  }

  getTourKeypoints(): void{
    this.tourAuthoringService.getKeypointsByTour(this.tourId).subscribe(res => {
      this.keypoints = res.results;
      this.routeQuery = {
        keypoints: this.keypoints,
        transportType: this.tour.transportType
      }
    });
    this.mode = 'add';
    this.saveTour(false);
  }

  setTourRoute(event: RouteInfo){
    if(this.tour.duration !== event.duration || this.tour.distance !== event.distance){
      this.tour.duration = event.duration;
      this.tour.distance = event.distance;

      this.tourAuthoringService.updateTour(this.tour).subscribe({
        next: (updatedTour) => { 
          this.tour = updatedTour;
        }
      });
     }
  }

  selectPublicKeypoint(keypoint: Keypoint) {
    this.openPublicKeypointList = false;
    keypoint.id = 0;
    keypoint.tourId = this.tour.id;
    this.tourAuthoringService.addKeypoint(keypoint).subscribe({
      next: () => { 
        this.getTourKeypoints();
      }
    });
  }
}
