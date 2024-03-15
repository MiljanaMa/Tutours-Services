import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Encounter } from '../model/encounter.model';
import { EncountersService } from '../encounters.service';
import { PagedResults } from 'src/app/shared/model/paged-results.model';

@Component({
  selector: 'xp-encounters-requests',
  templateUrl: './encounters-requests.component.html',
  styleUrls: ['./encounters-requests.component.css']
})
export class EncountersRequestsComponent {
  @Input() encounterRequests : Encounter[];
  public selectedEncounterRequest: Encounter;
  @Output() approvalStatusChanged = new EventEmitter<null>();

  constructor(private encountersService: EncountersService) { }

  approve(encounter: Encounter): void{
    this.encountersService.approve(encounter).subscribe({
      next: () => {
        this.approvalStatusChanged.emit();
      },
      error: () => {
        
      }
    });
  }

  decline(encounter: Encounter): void {
    this.encountersService.decline(encounter).subscribe({
      next: () => {
        this.approvalStatusChanged.emit();
      },
      error: () => {
        
      }
    });
  }
}
