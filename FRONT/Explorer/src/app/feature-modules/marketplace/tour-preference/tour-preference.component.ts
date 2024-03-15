import { Component, OnInit } from '@angular/core';
import { TourPreference } from '../model/tour-preference.model';
import { MarketplaceService } from '../marketplace.service';

@Component({
  selector: 'xp-tour-preference',
  templateUrl: './tour-preference.component.html',
  styleUrls: ['./tour-preference.component.css']
})
export class TourPreferenceComponent implements OnInit {

  tourPreference: TourPreference | undefined;
  mode: string = 'preview';
  shouldRenderTourPreferencesForm: boolean = false;

  constructor(private service: MarketplaceService) { }

  ngOnInit(): void {
    this.getTourPreference();
  }

  getTourPreference(): void {
    this.tourPreference = undefined;
    this.shouldRenderTourPreferencesForm = false;
    this.mode = 'preview';
    this.service.getTourPreference().subscribe({
      next: (result: TourPreference) => { 
        this.tourPreference = result;
      },
      error: () => {
      }
    });
  }

  addTourPreference(): void {
    this.mode = 'add';
    this.shouldRenderTourPreferencesForm = true;
  }

  editTourPreference(): void {
    this.mode = 'edit';
    this.shouldRenderTourPreferencesForm = true;
  }

  deleteTourPreference(): void {
    if(this.tourPreference) {
      this.service.deleteTourPreference().subscribe({
        next: () => { 
          this.getTourPreference();
        }
      });
    }
  }
}
