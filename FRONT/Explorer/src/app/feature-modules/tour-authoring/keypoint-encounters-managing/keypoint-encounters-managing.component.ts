import { Component, Input, OnChanges, OnInit, Output } from '@angular/core';
import { KeypointEncounter } from '../model/keypointEncounter.model';
import { TourAuthoringService } from '../tour-authoring.service';
import { PagedResults } from 'src/app/shared/model/paged-results.model';
import { Keypoint } from '../model/keypoint.model';

@Component({
  selector: 'xp-keypoint-encounters-managing',
  templateUrl: './keypoint-encounters-managing.component.html',
  styleUrls: ['./keypoint-encounters-managing.component.css']
})
export class KeypointEncountersManagingComponent implements OnInit, OnChanges {
  @Input() selectedKeypoint: Keypoint;
  //@Input() selectedKeypointChanged: Keypoint;
  public keypoint: Keypoint;
  public encounters: KeypointEncounter[];
  public selectedEncounter: KeypointEncounter;
  public showForm: boolean;
  public formMode: string;

  constructor(private tourauthoringService: TourAuthoringService ) {
    this.showForm = false;
    this.formMode = 'add';
  }

  ngOnInit(): void {
    this.keypoint = this.selectedKeypoint;
    this.getKeypointEncounters(this.keypoint.id || 0);
  }
  ngOnChanges(): void {
    this.keypoint = this.selectedKeypoint;
    this.getKeypointEncounters(this.keypoint.id || 0);
  }
  getKeypointEncounters(keypointId: number): void {
    this.tourauthoringService.getKeypointEncounters(keypointId).subscribe({
      next: (result: PagedResults<KeypointEncounter>) => {
        this.encounters = result.results;
      }
    });
  }

  addEncounter(): void {
    this.showForm = true;
    this.formMode = 'add';
  }

  selectEncounter(encounter: KeypointEncounter): void {
    this.selectedEncounter = encounter;
    this.formMode = 'edit';
    this.showForm = true;
  }

  encounterUpdated(): void {
    this.getKeypointEncounters(this.keypoint.id || 0);
    this.showForm = false;
  }

  encounterDeleted(): void {
    this.getKeypointEncounters(this.keypoint.id || 0);
  }

}
