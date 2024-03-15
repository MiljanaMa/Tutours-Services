import { Component, Input } from '@angular/core';
import { Club } from '../model/club.model';
import { Router } from '@angular/router';
import { TouristService } from '../tourist.service';
import { ClubChallengeRequest, ClubChallengeStatus } from '../model/club-challenge-request';
import { Profile } from '../../administration/model/profile.model';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';
import { ProfileService } from '../../administration/profile.service';

@Component({
  selector: 'xp-club-card',
  templateUrl: './club-card.component.html',
  styleUrls: ['./club-card.component.css']
})
export class ClubCardComponent{

  @Input() club: Club = {} as Club;
  public loggedInUser: Profile;

  constructor(private router: Router, private touristService: TouristService, private authService: AuthService, private profileService: ProfileService){
    this.profileService.getPerson(this.authService.user$.value.id).subscribe(res => {
      this.loggedInUser = res;
    });
  }

  openDetails(clubId: number){
    this.router.navigate([
      '/club', clubId
    ]);
  }

  challenge(clubId: number){
    if(!this.loggedInUser.clubId){
      window.alert('You are not a member of any club');
      return;
    }

    let challengeRequest: ClubChallengeRequest = {
      challengerId: this.loggedInUser.clubId || 0,
      challengedId: clubId,
      status: ClubChallengeStatus.PENDING
    }
    this.touristService.createChallenge(challengeRequest).subscribe({
      next(value) {
        window.alert('Challenge request sent');
      },

      error(err) {
        window.alert('Error during challenge request');
      },
    });
  }
}
