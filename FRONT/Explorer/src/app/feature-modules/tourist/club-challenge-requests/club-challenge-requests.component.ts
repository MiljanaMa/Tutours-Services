import { Component, Input } from '@angular/core';
import { ClubChallengeRequest } from '../model/club-challenge-request';
import { TouristService } from '../tourist.service';
import { Router } from '@angular/router';

@Component({
  selector: 'xp-club-challenge-requests',
  templateUrl: './club-challenge-requests.component.html',
  styleUrls: ['./club-challenge-requests.component.css']
})
export class ClubChallengeRequestsComponent {
  @Input() requests: ClubChallengeRequest[] | undefined = [];
  @Input() ownerPanel: Boolean = false;

  constructor(private touristService: TouristService, private router: Router){}

  acceptRequest(request: ClubChallengeRequest){
    if(window.confirm('Confirm that you want to accept challenge')){
      this.touristService.acceptChallenge(request).subscribe(res => {
        window.alert('Challenge accepted. The fight has begun');
      });
    }
  }

  declineRequest(request: ClubChallengeRequest){
    if(window.confirm('Confirm that you want to decline challenge')){
      this.touristService.declineChallenge(request).subscribe({
        next: (res) => {
          window.alert('Challenge accepted. The fight has begun');
          this.router.navigate(
            ['/fight', res.id]
          );
        },

        error: (err) => {
          window.alert(err);
        }
      });
    }
  }
}
