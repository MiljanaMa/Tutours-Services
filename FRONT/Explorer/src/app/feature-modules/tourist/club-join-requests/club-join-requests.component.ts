import { Component, OnInit } from '@angular/core';
import { ClubJoinRequest } from '../model/club-join-request.model';
import { TouristService } from '../tourist.service';
import { PagedResults } from 'src/app/shared/model/paged-results.model';

@Component({
  selector: 'xp-club-join-requests',
  templateUrl: './club-join-requests.component.html',
  styleUrls: ['./club-join-requests.component.css']
})
export class ClubJoinRequestsComponent implements OnInit {
  public joinRequests : ClubJoinRequest[];

  constructor(private touristService: TouristService){}

  ngOnInit(): void {
    this.getRequests(); 
  }
  getRequests(): void{
    this.touristService.getTouristRequests().subscribe({
      next: (response: PagedResults<ClubJoinRequest>) => {
        this.joinRequests = response.results;
      },
      error: () => {
        
      }
    });
  }
  cancelRequest(request: ClubJoinRequest): void {
    request.status = 3;
    this.touristService.updateRequest(request).subscribe({
    });
  }

}
