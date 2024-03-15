import { Component, OnInit } from '@angular/core';
import { ClubInvitation } from '../model/club-invitation.model';
import { TouristService } from '../tourist.service';
import { PagedResults } from '../../../shared/model/paged-results.model';
import { Club } from '../model/club.model';

@Component({
  selector: 'xp-club-invitation',
  templateUrl: './club-invitation.component.html',
  styleUrls: ['./club-invitation.component.css']
})
export class ClubInvitationComponent implements OnInit {

  clubInvitations: ClubInvitation[] = [];
  clubs: Club[] = [];
  selectedClub: Club;
  shouldRenderClubInvitationForm: boolean = false;
  shouldRenderClubRequestView: boolean = false;

  constructor(private service: TouristService) { }

  ngOnInit(): void {
    this.getClubs();
    this.getAllInvitations();
  }

  getClubs(): void {
    this.service.getClubs().subscribe({
      next: (result: PagedResults<Club>) => {
        this.clubs = result.results;
      },
      error: (err: any) => {
        console.log(err)
      }
    })
  }

  getAllInvitations(): void {
    this.service.getClubInvitations().subscribe({
      next: (result: PagedResults<ClubInvitation>) => {
        this.clubInvitations = result.results;
      },
      error: (err: any) => {
        console.log(err)
      }
    })
  }

  getInvitationsByClub(clubId: number): ClubInvitation[] {
    return this.clubInvitations.filter((invitation) => invitation.clubId === clubId);
  }

  onAddClicked(club: Club): void {
    this.selectedClub = club;
    this.shouldRenderClubInvitationForm = true;
    this.shouldRenderClubRequestView = false;
  }

  removeMember(club: Club, memberId: number): void {
    // const startIndex = club.memberIds.indexOf(memberId);
    const deleteCount = 1;

    // if (startIndex !== -1) {
    //   // club.memberIds.splice(startIndex, deleteCount);
    // }

    this.service.updateClub(club).subscribe({});
  }
  showRequests(club: Club): void {
    this.selectedClub = club;
    this.shouldRenderClubRequestView = true;
    this.shouldRenderClubInvitationForm = false;
  }
}
