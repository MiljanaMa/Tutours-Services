import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Keypoint } from '../model/keypoint.model';
import { TourAuthoringService } from '../tour-authoring.service';
import { PublicEntityRequest } from '../model/public-entity-request.model';

@Component({
  selector: 'xp-keypoint',
  templateUrl: './keypoint.component.html',
  styleUrls: ['./keypoint.component.css']
})
export class KeypointComponent implements OnInit{

  @Output() keypointDeleted = new EventEmitter<null>();
  @Output() keypointSelected = new EventEmitter<Keypoint>();
  @Input() keypoints : Keypoint[];
  @Input() isCustom : Boolean = false;
  public selectedKeypoint: Keypoint;
  publicEntityRequest: PublicEntityRequest;
  newPublicEntityRequest: PublicEntityRequest;


  constructor(private tourAuthoringService: TourAuthoringService){}

  ngOnInit(): void {
    
  }

  deleteKeypoint(id: number): void{
    if(window.confirm('Are you sure that you want to delete this keypoint?')){
      this.tourAuthoringService.deleteKeypoint(id).subscribe({
        next: () => {
          if(!this.isCustom){
            this.tourAuthoringService.deleteKeypointEncounters(id).subscribe({
              next: () => {
                this.keypointDeleted.emit();
              }
            });
          }
          this.keypointDeleted.emit();
        },
        error: () => {
          
        }
      });
    }
  }

  onEditClicked(keypoint: Keypoint): void{
    this.keypointSelected.emit(keypoint);
  }
  onEncountersClicked(keypointId: number): void{
    //this.keypointSelected.emit(keypoint);
  }

  sendPublicEntityRequest(id: number): void{
    this.tourAuthoringService.getPublicEntityRequestByEntityId(id, 0).subscribe({
      next: (result: PublicEntityRequest) => { 
        this.publicEntityRequest.id = result.id;
        this.publicEntityRequest.entityId = result.entityId;
        this.publicEntityRequest.entityType = result.entityType;
        this.publicEntityRequest.status = result.status;
        this.publicEntityRequest.comment = result.comment;
      },
      complete: () => {
        if (
          this.publicEntityRequest &&
          this.publicEntityRequest.entityId == id &&
          this.publicEntityRequest.entityType == 0 
        ) {
          window.alert('Request for this keypoint already exists!');
        } else if (this.publicEntityRequest == null ) {
          if (window.confirm('Are you sure that you want this keypoint to be public?')) {
            this.newPublicEntityRequest = {
              entityId: id,
              entityType: 0,
              status: 0,
              comment: '',
            };
            this.tourAuthoringService.addPublicEntityRequestKeypoint(this.newPublicEntityRequest).subscribe({
              next: () => {
                window.alert('You have successfully sent a request for making this keypoint public');
              },
            });
          }
        }
      }
    }); 
  }
}