import { Component, OnInit } from '@angular/core';
import { Object } from '../model/object.model';
import { TourAuthoringService } from '../tour-authoring.service';
import { PagedResults } from 'src/app/shared/model/paged-results.model';
import { PublicEntityRequest } from '../model/public-entity-request.model';

@Component({
  selector: 'xp-object',
  templateUrl: './object.component.html',
  styleUrls: ['./object.component.css']
})
export class ObjectComponent implements OnInit{

    objects: Object[] = [];
    selectedObject: Object;
    mode : string = 'add';
    renderObject: boolean = false;
    publicEntityRequest: PublicEntityRequest;
    newPublicEntityRequest: PublicEntityRequest;

    constructor(private tourAuthoringService: TourAuthoringService){ }

    ngOnInit(): void{
      this.getObjects();
    }

    deleteObject(id: number): void{
      if(window.confirm('Are you sure that you want to delete this object?')){
        this.tourAuthoringService.deleteObject(id).subscribe({
          next: () => {
            this.getObjects();
          },
          error: () => {
            
          }
        });
      }
    }

    onEditClicked(object: Object):void{
      this.selectedObject = object;
      console.log(this.selectedObject);
      this.mode = 'edit';
      this.renderObject = true;
    }

    onAddClicked(): void{
      this.mode = 'add';
      this.renderObject = true;
    }
    

    getObjects(): void{
      this.tourAuthoringService.getObjects().subscribe({
        next: (response: PagedResults<Object>) => {
          this.objects = response.results;
        },
        error: () => {
          
        }
      });
    }
    
    onSendRequestClicked(id: number): void {
      this.tourAuthoringService.getPublicEntityRequestByEntityId(id, 1).subscribe({
        next: (result: PublicEntityRequest) => {
          this.publicEntityRequest = result;
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
            this.publicEntityRequest.entityType == 1
            ) {
            window.alert('Request for this object already exists!');
          } else if (this.publicEntityRequest == null ) {
            if (window.confirm('Are you sure that you want this object to be public?')) {
              this.newPublicEntityRequest = {
                entityId: id,
                entityType: 1,
                status: 0,
                comment: '',
              };
              this.tourAuthoringService.addPublicEntityRequestObject(this.newPublicEntityRequest).subscribe({
                next: () => {
                  window.alert('You have successfully sent a request for making this object public');
                },
              });
            }
          }
        },
      });
    }
}
