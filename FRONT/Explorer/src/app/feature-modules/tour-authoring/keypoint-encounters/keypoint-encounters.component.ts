import { Component, EventEmitter, Input, Output } from '@angular/core';
import { KeypointEncounter } from '../model/keypointEncounter.model';
import { TourAuthoringService } from '../tour-authoring.service';

@Component({
  selector: 'xp-keypoint-encounters',
  templateUrl: './keypoint-encounters.component.html',
  styleUrls: ['./keypoint-encounters.component.css']
})
export class KeypointEncountersComponent{

  @Output() encounterDeleted = new EventEmitter<null>();
  @Output() encounterSelected = new EventEmitter<KeypointEncounter>();
  @Input() encounters : KeypointEncounter[];
  @Input() showActions: boolean = false;

  public selectedEncounter: KeypointEncounter;

  constructor(private tourAuthoringService: TourAuthoringService){ }

  deleteEncounter(id: number): void{
    if(window.confirm('Are you sure that you want to delete this encounter?')){
      this.tourAuthoringService.deleteEncounter(id).subscribe({
        next: () => {
          this.encounterDeleted.emit();
        },
        error: () => {
          
        }
      });
    }
  }

  editEncounter(encounter: KeypointEncounter): void {
    this.encounterSelected.emit(encounter);
  }
}
