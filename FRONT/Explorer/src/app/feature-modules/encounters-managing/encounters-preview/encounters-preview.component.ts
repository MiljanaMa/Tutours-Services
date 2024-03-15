import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Encounter } from '../model/encounter.model';
import { EncountersService } from '../encounters.service';

@Component({
  selector: 'xp-encounters-preview',
  templateUrl: './encounters-preview.component.html',
  styleUrls: ['./encounters-preview.component.css']
})
export class EncountersPreviewComponent {
  @Output() encounterDeleted = new EventEmitter<null>();
  @Output() encounterSelected = new EventEmitter<Encounter>();
  @Input() encounters : Encounter[];
  @Input() showActions: boolean = false;

  public selectedEncounter: Encounter;

  constructor(private encountersService: EncountersService) { }

  deleteEncounter(id: number): void{
    if(window.confirm('Are you sure that you want to delete this encounter?')){
      this.encountersService.deleteEncounter(id).subscribe({
        next: () => {
          this.encounterDeleted.emit();
        },
        error: () => {
          
        }
      });
    }
  }

  editEncounter(encounter: Encounter): void {
    this.encounterSelected.emit(encounter);
  }
}
