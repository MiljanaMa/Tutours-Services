import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { PublicEntityRequest } from '../../tour-authoring/model/public-entity-request.model';
import { AdministrationService } from '../administration.service';
import { PagedResults } from 'src/app/shared/model/paged-results.model';


@Component({
  selector: 'xp-public-entity-requests',
  templateUrl: './public-entity-requests.component.html',
  styleUrls: ['./public-entity-requests.component.css']
})
export class PublicEntityRequestsComponent implements OnInit{

  requests: PublicEntityRequest[] = [];
  constructor(private service: AdministrationService) {  }

  ngOnInit(): void {
    this.getRequests();
  }

  getRequests(): void{
    this.service.getAllRequests().subscribe({
      next: (result: PagedResults<PublicEntityRequest>) => {
        this.requests = result.results
      },
      error: (err: any) => {
        console.log(err)
      }
    })
  }

  approveRequest(request: PublicEntityRequest): void{
    if(window.confirm('Are you sure that you want to approve this request?')){
      this.service.apporoveRequest(request).subscribe({
        next: () => {
          window.alert('Request is approved');
          this.getRequests();
        },
        error: () => {
          window.alert('Request has already been accepted or declined');
        }
      });
    }
  }

  declineRequest(request: PublicEntityRequest): void{
    if(window.confirm('Are you sure that you want to decline this request?')){
      let comment = prompt('Enter comment why are you declining this request!');
      if(comment != null){
        request.comment = comment;
      }
      this.service.declineRequest(request).subscribe({
        next: () => {
          window.alert('Request is declined');
          this.getRequests();
        },
        error: () => {
          window.alert('Request has already been accepted or declined');
        }
      });
    }
  }

  getEntityTypeString(entityType: number): string {
    return entityType === 0 ? 'KEYPOINT' : 'OBJECT';
  }
  
  getStatusString(status: number): string {
    switch (status) {
      case 0:
        return 'PENDING';
      case 1:
        return 'APPROVED';
      case 2:
        return 'DECLINED';
      default:
        return 'Unknown'; 
    }
  }
  
  
}
