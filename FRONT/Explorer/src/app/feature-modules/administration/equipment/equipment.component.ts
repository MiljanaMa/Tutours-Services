import { Component, OnInit } from '@angular/core';
import { AdministrationService } from '../administration.service';
import { Equipment } from '../model/equipment.model';
import { PagedResults } from 'src/app/shared/model/paged-results.model';


@Component({
  selector: 'xp-equipment',
  templateUrl: './equipment.component.html',
  styleUrls: ['./equipment.component.css']
})
export class EquipmentComponent implements OnInit {

  equipment: Equipment[] = [];
  selectedEquipment: Equipment;
  shouldRenderEquipmentForm: boolean = false;
  shouldEdit: boolean = false;
  
  constructor(private service: AdministrationService) { }

  ngOnInit(): void {
    this.getEquipment();
  }
  
  deleteEquipment(id: number): void {
    this.service.deleteEquipment(id).subscribe({
      next: () => {
        this.getEquipment();
      },
    })
  }

  getEquipment(): void {
    this.service.getEquipment().subscribe({
      next: (result: PagedResults<Equipment>) => {
        this.equipment = result.results;
      },
      error: () => {
      }
    })
  }

  onEditClicked(equipment: Equipment): void {
    this.selectedEquipment = equipment;
    this.shouldRenderEquipmentForm = true;
    this.shouldEdit = true;
  }

  onAddClicked(): void {
    this.shouldEdit = false;
    this.shouldRenderEquipmentForm = true;
  }
}
