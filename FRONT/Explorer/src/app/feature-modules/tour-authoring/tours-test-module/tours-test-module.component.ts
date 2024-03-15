import { Component } from '@angular/core';
import { MapComponent } from 'src/app/shared/map/map.component';
import { TestTour } from 'src/app/shared/model/testtour.model';

@Component({
  selector: 'xp-tours-test-module',
  templateUrl: './tours-test-module.component.html',
  styleUrls: ['./tours-test-module.component.css']
})
export class ToursTestModuleComponent {

  public tours: TestTour[];
  public selectedTour: TestTour;

  constructor(){
    this.tours = [];
    this.tours.push({name: 'Tour 1', keypoints: 
    [
      {name: 'endme', latitude: 45.245845, longitude: 19.851025},
      {name: 'endme', latitude: 45.234295, longitude: 19.850368},
    ]});

    this.tours.push({name: 'Tour 2', keypoints: 
    [
      {name: 'endme', latitude: 45.245845, longitude: 19.851025},
      {name: 'endme', latitude: 45.234295, longitude: 19.850368},
    ]});

    this.tours.push({name: 'Tour 3', keypoints: 
    [
      {name: 'endme', latitude: 45.245845, longitude: 19.851025},
      {name: 'endme', latitude: 45.234295, longitude: 19.850368},
    ]});
  }

  selectTour(t: TestTour): void{
    this.selectedTour = t;
  }
}

