import { Component, OnInit } from '@angular/core';
import { Status, Tour, TourDifficulty, TransportType } from '../model/tour.model';
import { TourAuthoringService } from '../tour-authoring.service';

@Component({
  selector: 'xp-tours-preview',
  templateUrl: './tours-preview.component.html',
  styleUrls: ['./tours-preview.component.css']
})
export class ToursPreviewComponent implements OnInit{
  public tours: Tour[] = [];
  public renderTourEquipment: boolean = false;
  public selectedTour: Tour;

  constructor(private tourAuthoringService: TourAuthoringService){}

  ngOnInit(): void {
    this.getTours();
  }
  
  
  getTours(): void {
    this.tourAuthoringService.getTours().subscribe(pagedResults => this.tours = pagedResults.results);
  }

  onEditClicked(tour: Tour): void{
    this.selectedTour = tour;
    console.log(this.selectedTour);
    //this.mode = 'edit';
    this.renderTourEquipment = true;
  }

}
