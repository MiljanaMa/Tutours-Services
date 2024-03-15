import { Component, EventEmitter, Input, OnChanges, OnInit, Output } from '@angular/core';
import { Keypoint } from '../model/keypoint.model';
import { TourAuthoringService } from '../tour-authoring.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { RouteQuery } from 'src/app/shared/model/routeQuery.model';
import { RouteInfo } from 'src/app/shared/model/routeInfo.model';
import { PagedResults } from 'src/app/shared/model/paged-results.model';
import { Location } from '../model/location.model';

@Component({
  selector: 'xp-keypoint-form',
  templateUrl: './keypoint-form.component.html',
  styleUrls: ['./keypoint-form.component.css']
})
export class KeypointFormComponent implements OnChanges, OnInit {

  @Output() keypointsUpdated = new EventEmitter<Keypoint>();
  @Output() routeFound = new EventEmitter<RouteInfo>();

  @Input() tourId: number;
  @Input() routeQuery: RouteQuery;
  @Input() selectedKeypoint: Keypoint;
  @Input() keypointsCount: number = 0;
  @Input() mode: string = 'add';

  public publicKeypoints: Keypoint[];
  public keypointForm: FormGroup;
  public locationChanged: boolean = false;

  public openPublicKeypointList: boolean;

  constructor(private tourAuthoringService: TourAuthoringService) {
    this.keypointForm = new FormGroup({
      name: new FormControl('', [Validators.required]),
      latitude: new FormControl(0, [Validators.min(-90), Validators.max(90)]),
      longitude: new FormControl(0, [Validators.min(-180), Validators.max(180)]),
      description: new FormControl(''),
      image: new FormControl(''),
      secret: new FormControl(''), 
      position: new FormControl(this.keypointsCount+1, [Validators.required, Validators.min(1)])
    });
    this.openPublicKeypointList = false;
  }

  ngOnInit(): void {
    this.getPublicKeypoints();
  }

  ngOnChanges(): void {
    this.keypointForm.reset();
    this.keypointForm.patchValue({'position': this.keypointsCount + 1});
    if(this.mode === 'edit') {
      this.keypointForm.patchValue(this.selectedKeypoint);
    }
  }

  saveKeypoint(): void {
    if(this.tourId === 0){
      window.alert("You can not add keypoint to undrafted tour");
    }else if(!this.keypointForm.valid){
      window.alert("Form invalid");
    }else{
      let keypoint: Keypoint = {
        tourId: this.tourId,
        name: this.keypointForm.value.name || "",
        latitude: this.keypointForm.value.latitude || 0,
        longitude: this.keypointForm.value.longitude || 0,
        description: this.keypointForm.value.description || "",
        image: this.keypointForm.value.image || "",
        secret: this.keypointForm.value.secret || "", 
        position: this.keypointForm.value.position || 0
      };
      
      if(this.mode === 'add'){
        this.tourAuthoringService.addKeypoint(keypoint).subscribe({
          next: () => { 
            this.keypointsUpdated.emit();
            this.keypointForm.reset();
            this.keypointForm.clearValidators();
          }
        });
      }else if( this.mode === 'edit'){
        keypoint.id = this.selectedKeypoint.id;
        this.tourAuthoringService.updateKeypoint(keypoint).subscribe({
          next: () => {
            if(this.locationChanged){
              let location : Location = {
                longitude: keypoint.longitude,
                latitude: keypoint.latitude,
              };
              this.tourAuthoringService.updateEncountersLocation(keypoint.id || 0, location).subscribe({
                  next: () => {

                  }
              });
            }
            window.alert(`You have successfuly updated ${keypoint.name}`);
            this.keypointsUpdated.emit(); 
            this.keypointForm.reset();
            this.keypointForm.clearValidators();
          }
        });
      } 
    }
  }

  getPublicKeypoints() {
    this.tourAuthoringService.getPublicKeypoints().subscribe({
      next: (result: PagedResults<Keypoint>) => {
        this.publicKeypoints = result.results;
      }
    });
  }

  selectPublicKeypoint(keypoint: Keypoint) {
    this.keypointForm.patchValue(keypoint);
    this.openPublicKeypointList = false;
  }

  fillCoords(event: number[]): void {
    this.locationChanged = true;
    this.keypointForm.patchValue({
      latitude: event[0],
      longitude: event[1]
    })
  }

  routeFoundEmit(routeInfo: RouteInfo){
    this.routeFound.emit(routeInfo);
  }
}
