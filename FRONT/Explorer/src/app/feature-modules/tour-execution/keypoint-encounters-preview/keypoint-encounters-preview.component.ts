import { Component, Input } from '@angular/core';
import { KeypointEncounter } from '../../tour-authoring/model/keypointEncounter.model';
import { TourExecutionService } from '../tour-execution.service';
import { EncounterCompletion } from '../../encounters-managing/model/encounterCompletion.model';
import { EncountersService } from '../../encounters-managing/encounters.service';
import { Encounter } from '../../encounters-managing/model/encounter.model';

@Component({
  selector: 'xp-keypoint-encounters-preview',
  templateUrl: './keypoint-encounters-preview.component.html',
  styleUrls: ['./keypoint-encounters-preview.component.css']
})
export class KeypointEncountersPreviewComponent {
  @Input() keypointEncounters: KeypointEncounter[];
  @Input() requiredEncounters: KeypointEncounter[];
  public sortDirection: number = 0;

  constructor(private tourExecutionService: TourExecutionService, private encounterService: EncountersService){ }

  sortTable(field: string) {
    if(this.sortDirection === 0)
      this.sortDirection = 1;
    else
      this.sortDirection = 0;
    this.sortData(field);
  }
  sortData(value: string) {
    let encounters = this.keypointEncounters;
    encounters.sort((a: any, b: any) => {
      if (this.sortDirection) 
      {
        if (a[value] > b[value]) {
          return -1;
        } else if (a[value] < b[value]) {
          return 1;
        } else {
          return 0;
        }
      } 
      else 
      {
        if (a[value] < b[value]) {
          return -1;
        } else if (a[value] > b[value]) {
          return 1;
        } else {
          return 0;
        }
      }
    });
    this.keypointEncounters = encounters;
  }
  startEncounter(encounter: Encounter): void {
    this.encounterService.startEncounter(encounter).subscribe({
      next: (result: EncounterCompletion) =>{
          if (window.confirm("You started encounter")) {}
          //maybe add button to change on complete if is misc
      },
      error: (error) => {
        if (window.confirm("You have been started this encounter or completed")) {}
      }
    });
}

}
