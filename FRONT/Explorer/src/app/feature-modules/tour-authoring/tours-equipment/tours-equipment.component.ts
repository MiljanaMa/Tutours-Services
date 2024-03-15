import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { Tour } from '../model/tour.model';
import { Equipment } from '../../administration/model/equipment.model';
import { TourAuthoringService } from '../tour-authoring.service';
import { TourEquipment } from '../model/tour_equipment';

@Component({
  selector: 'xp-tours-equipment',
  templateUrl: './tours-equipment.component.html',
  styleUrls: ['./tours-equipment.component.css']
})
export class ToursEquipmentComponent implements OnInit {
  @Input() tour: Tour;
  allEquipment: Equipment[] = [];
  selectedEquipment: Equipment[] = [];
  availableEquipment: Equipment[] = [];
  tourEquipment: TourEquipment;

  constructor(private tourAuthoringService: TourAuthoringService) { }

  ngOnInit(): void {
    this.loadEquipmentData();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['tour'] && !changes['tour'].firstChange) {
      this.loadEquipmentData();
    }
  }

  loadEquipmentData() {
    this.tourAuthoringService.getEquipment().subscribe(pagedResults => {
      this.allEquipment = pagedResults.results;
      if (this.tour && this.tour.id) {
        this.tourAuthoringService.getEquipmentForTour(this.tour.id).subscribe(pagedResults => {
          this.selectedEquipment = pagedResults;
          console.log(this.selectedEquipment);
          this.availableEquipment = this.allEquipment.filter(e => !this.selectedEquipment.some(se => se.id === e.id));
        });
      }
      console.log(this.allEquipment);
    });
  }
  

  addToSelected(equipment: Equipment):void{
    this.tourEquipment = {
      tourId: this.tour.id,
      equipmentId: equipment.id
    }
    this.tourAuthoringService.addEquipmentToTour(this.tourEquipment).subscribe({
      next: () => {
        this.selectedEquipment.push(equipment);
        this.availableEquipment = this.allEquipment.filter(e => !this.selectedEquipment.includes(e));
      },
      error: () => {
        
      }
    });
  }
  removeFromSelected(equipment: Equipment):void{
    this.tourEquipment = {
      tourId: this.tour.id,
      equipmentId: equipment.id
    }
    this.tourAuthoringService.removeEquipmentFromTour(this.tourEquipment).subscribe({
      next: () => {
        this.selectedEquipment = this.selectedEquipment.filter(e => e.id !== equipment.id);
        this.availableEquipment.push(equipment);
      },
      error: () => {
        
      }
    });
  }

}
