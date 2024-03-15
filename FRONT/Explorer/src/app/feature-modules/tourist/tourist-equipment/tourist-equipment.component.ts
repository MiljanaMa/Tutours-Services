import { Component } from '@angular/core';
import { EquipmentForSelection } from '../model/eqipment-for-selection.model';
import { TouristService } from '../tourist.service';
import { TouristEquipment } from '../model/tourist-equipment.model';

@Component({
  selector: 'xp-tourist-equipment',
  templateUrl: './tourist-equipment.component.html',
  styleUrls: ['./tourist-equipment.component.css']
})
export class TouristEquipmentComponent {
  equipment: EquipmentForSelection[] = [];
  constructor(private service: TouristService) {

   }

  ngOnInit(): void {
    this.getEquipment();
  }
  getEquipment(): void {
    if(localStorage.getItem('loggedId') === null){
      return;
    }
    this.service.getEquipmentForSelection().subscribe({
      next: (result: EquipmentForSelection[]) => {
        this.equipment = result;
      },
      error: () => {
      }
    })
  }

  selectEquipment(equipmentId: any, equipmentSelected: boolean): void{
    if(localStorage.getItem('loggedId') === null){
      return;
    }
    const data: TouristEquipment = {
      touristId: parseInt(localStorage.getItem('loggedId')!),
      equipmentId : equipmentId,
    }
    if(equipmentSelected === false){
      this.service.createSelectionEquipment(data).subscribe({
        next: () => {  }
      });
    }else{
      this.service.deleteSelectionEquipment(data).subscribe({
        next: () => {  }
      });
    }
    

  }
}
