import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TouristService } from '../tourist.service';
import { ClubInvitation } from '../model/club-invitation.model';
import { Club } from '../model/club.model';

@Component({
  selector: 'xp-club-invitation-form',
  templateUrl: './club-invitation-form.component.html',
  styleUrls: ['./club-invitation-form.component.css']
})
export class ClubInvitationFormComponent implements OnChanges {

  @Output() clubInvitationsUpdated = new EventEmitter<null>();
  @Input() club: Club;
  errorMessage: string = '';

  constructor(private service: TouristService) { }

  ngOnChanges(changes: SimpleChanges): void {
    const clubInvitation : ClubInvitation ={
      clubId: this.club.id,
      userId: 0
    };
    this.clubInvitationForm.patchValue(clubInvitation);
  }

  clubInvitationForm = new FormGroup({
    clubId: new FormControl(0, [Validators.required]),
    userId: new FormControl(0, [Validators.required])
  })

  addClubInvitation(): void {
    this.errorMessage = '';
    const userIdToAdd = this.clubInvitationForm.value.userId || 0;

    // if (this.club.memberIds.includes(userIdToAdd)) {
    //   this.errorMessage = 'User is already a member of the club';
    //   return;
    // }

    const clubInvitation: ClubInvitation = {
      clubId: this.club.id || 0,
      userId: userIdToAdd
    }

    this.service.addClubInvitation(clubInvitation).subscribe({
      next: (_) => {
        this.clubInvitationsUpdated.emit()
      }
    });

    //auto add without accepting invitations - JUST TEMPORARY
    // this.club.memberIds.push(clubInvitation.userId);
    this.service.updateClub(this.club).subscribe({});
  }
}
