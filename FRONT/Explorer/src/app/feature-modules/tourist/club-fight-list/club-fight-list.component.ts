import { Component, Input } from '@angular/core';
import { ClubFight } from '../model/club-fight.model';
import { Router } from '@angular/router';
import { Club } from '../model/club.model';

@Component({
  selector: 'xp-club-fight-list',
  templateUrl: './club-fight-list.component.html',
  styleUrls: ['./club-fight-list.component.css']
})
export class ClubFightListComponent {
  @Input() fights: ClubFight[];
  @Input() club: Club;

  constructor(private router: Router) { }

  openFightDetails(fightId?: number)
  {
    this.router.navigate(
      ['/fight', fightId || 0]
    );
  }
}
