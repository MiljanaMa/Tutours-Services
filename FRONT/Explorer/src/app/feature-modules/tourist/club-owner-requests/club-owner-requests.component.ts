import { Component, Input, SimpleChanges } from '@angular/core';
import { ClubJoinRequest } from '../model/club-join-request.model';
import { TouristService } from '../tourist.service';
import { PagedResults } from 'src/app/shared/model/paged-results.model';
import { Club } from '../model/club.model';

@Component({
  selector: 'xp-club-owner-requests',
  templateUrl: './club-owner-requests.component.html',
  styleUrls: ['./club-owner-requests.component.css']
})
export class ClubOwnerRequestsComponent {

  public joinRequests : ClubJoinRequest[];
  @Input() club: Club;

  constructor(private touristService: TouristService){}
  acceptRequest(request: ClubJoinRequest): void {
    request.status = 0;
    this.touristService.updateRequest(request).subscribe({
      next: () => {
        // this.club.memberIds.push(request.userId);
        this.touristService.updateClub(this.club).subscribe({});
      },
      error: () => { }
    });
  }
  rejectRequest(request: ClubJoinRequest): void {
    request.status = 1;
    this.touristService.updateRequest(request).subscribe({
    });
  }
  ngOnChanges(changes: SimpleChanges): void {
    this.touristService.getClubRequests(this.club.id).subscribe({
      next: (response: PagedResults<ClubJoinRequest>) => {
        this.joinRequests = response.results;
      },
      error: () => { }
    });
  }

}
