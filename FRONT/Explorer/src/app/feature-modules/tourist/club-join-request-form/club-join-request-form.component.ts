import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TouristService } from '../tourist.service';
import { ClubJoinRequest } from '../model/club-join-request.model';

@Component({
  selector: 'xp-club-join-request-form',
  templateUrl: './club-join-request-form.component.html',
  styleUrls: ['./club-join-request-form.component.css']
})
export class ClubJoinRequestFormComponent {
  public clubJoinRequestForm: FormGroup;

  constructor(private touristService: TouristService) {
    this.clubJoinRequestForm = new FormGroup({
      id: new FormControl('', [Validators.required]),
    });
  }
  sendRequest(): void {
    const clubIdToSend = this.clubJoinRequestForm.value.clubId || 0;

    /*const request: ClubJoinRequest = {
      clubId: this.club.id || 0,
      userId: userIdToAdd
    }

    this.service.addClubInvitation(clubInvitation).subscribe({
      next: (_) => {
        this.clubInvitationsUpdated.emit()
      }
    });

    //auto add without accepting invitations - JUST TEMPORARY
    this.service.updateClub(this.club).subscribe({});*/
  }
}
