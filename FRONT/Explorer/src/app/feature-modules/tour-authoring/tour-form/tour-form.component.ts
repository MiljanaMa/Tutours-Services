import { Component, EventEmitter, Input, OnChanges, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Status, Tour, TourDifficulty, TransportType } from '../model/tour.model';
import { TourAuthoringService } from '../tour-authoring.service';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { Keypoint } from '../model/keypoint.model';
import { RouteQuery } from 'src/app/shared/model/routeQuery.model';
import { RouteInfo } from 'src/app/shared/model/routeInfo.model';

@Component({
  selector: 'xp-tour-form',
  templateUrl: './tour-form.component.html',
  styleUrls: ['./tour-form.component.css']
})
export class TourFormComponent implements OnChanges, OnInit{  
  
  public tour: Tour;
  public tourForm: FormGroup;
  public tourId: number;
  public keypoints: Keypoint[] = [];
  public selectedKeypoint: Keypoint;
  public mode: string = 'add';
  public routeQuery: RouteQuery;
  @Output() selectedKeypointChanged = new EventEmitter<Keypoint>();

  constructor(private tourAuthoringService: TourAuthoringService, private router: Router, private route: ActivatedRoute) {
    this.tour = { description: '', difficulty: TourDifficulty.EASY, status: Status.DRAFT, name: '', price: 0, transportType: TransportType.WALK, userId: 0, id:0}
    this.tourForm = new FormGroup({
      name: new FormControl('', [Validators.required]),
      description: new FormControl(''),
      price: new FormControl(1, Validators.min(1)),
      difficulty: new FormControl(''),
      transportType: new FormControl(''),
      newTag: new FormControl('')
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
        }
      });
  }

  ngOnChanges(): void {
  }

  saveTour(statusChange: string = ''): void {
    let currentStatus = this.tourId === 0 ? Status.DRAFT : this.tour.status;
    let tour: Tour = {
      id: this.tourId,
      userId: -1,
      name: this.tourForm.value.name || "",
      description: this.tourForm.value.description || "",
      price: this.tourForm.value.price,
      difficulty: this.tourForm.value.difficulty || "",
      transportType: this.tourForm.value.transportType || "",
      status: currentStatus,
      tags: this.tour.tags,
    };

    if(this.tourId === 0){
      tour.price = 0;
      tour.statusUpdateTime = new Date();
      this.tourAuthoringService.addTour(tour).subscribe({
        next: (newTour) => { 
          window.alert("You have successfuly saved your tour");
          this.router.navigate(
            ['/tour-management', newTour.id]
          );
        }
      });
    }else{
      if(statusChange){
        if(!window.confirm(`Are you sure that you want to ${statusChange} this tour?`) || this.keypoints.length < 2){
          return;
        }
        tour.statusUpdateTime = new Date();
        tour.status = statusChange === 'publish' ? Status.PUBLISHED : Status.ARCHIVED;
      }else{
        tour.statusUpdateTime = this.tour.statusUpdateTime;
      }
      tour.distance = this.tour.distance;
      tour.duration = this.tour.duration;
      this.tourAuthoringService.updateTour(tour).subscribe({
        next: (updatedTour) => { 
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
  }

  selectKeypoint(event: Keypoint): void{
    this.selectedKeypoint = event;
    this.selectedKeypointChanged.emit();
    this.mode = 'edit';
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

  addTag(): void {
    if(!this.tourForm.value.newTag) return;

    this.tour.tags?.push(this.tourForm.value.newTag);
    this.tourForm.patchValue({newTag: ''});
  }

  removeTag(tag: string): void {
    this.tour.tags?.splice(this.tour.tags?.indexOf(tag), 1);
  }
}

