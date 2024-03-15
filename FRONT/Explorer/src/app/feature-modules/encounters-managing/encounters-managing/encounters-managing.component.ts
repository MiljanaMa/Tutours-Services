import { outputAst } from '@angular/compiler';
import { Component, OnInit } from '@angular/core';
import { Encounter } from '../model/encounter.model';
import { EncountersService } from '../encounters.service';
import { PagedResults } from 'src/app/shared/model/paged-results.model';

@Component({
  selector: 'xp-encounters-managing',
  templateUrl: './encounters-managing.component.html',
  styleUrls: ['./encounters-managing.component.css']
})
export class EncountersManagingComponent implements OnInit {
  public encounters: Encounter[];
  public selectedEncounter: Encounter;
  public showForm: boolean;
  public formMode: string;
  public encounterRequests: Encounter[];

  constructor(private encountersService: EncountersService) {
    this.showForm = false;
    this.formMode = 'add';
  }

  ngOnInit(): void {
    this.getEncounters();
    this.getEncounterRequests();
  }

  getEncounters(): void {
    this.encountersService.getEncounters().subscribe({
      next: (result: PagedResults<Encounter>) => {
        this.encounters = result.results;
      }
    });
  }

  getEncounterRequests(): void {
    this.encountersService.getEncounterRequests().subscribe({
      next: (result: PagedResults<Encounter>) => {
        this.encounterRequests = result.results;
      }
    });
  }

  addEncounter(): void {
    this.showForm = true;
    this.formMode = 'add';
  }

  selectEncounter(encounter: Encounter): void {
    this.selectedEncounter = encounter;
    this.formMode = 'edit';
    this.showForm = true;
  }

  encounterUpdated(): void {
    this.getEncounters();
    this.showForm = false;
  }

  encounterDeleted(): void {
    this.getEncounters();
    this.getEncounterRequests();
  }

  approvalStatusChanged(): void {
    this.getEncounters();
    this.getEncounterRequests();
  }
}
