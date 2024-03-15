import { Component, Input } from '@angular/core';
import { Person } from '../../administration/model/person.model';
import { Profile } from '../../administration/model/profile.model';

@Component({
  selector: 'xp-club-members',
  templateUrl: './club-members.component.html',
  styleUrls: ['./club-members.component.css']
})
export class ClubMembersComponent {

  @Input() members: Profile[] | undefined = [];
  @Input() ownerId: number | undefined;
  @Input() ownerPanel: Boolean = false;

  constructor(){}
}
