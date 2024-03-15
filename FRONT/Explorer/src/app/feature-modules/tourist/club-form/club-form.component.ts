import { Component, EventEmitter, OnChanges } from '@angular/core';
import { FormControl, FormGroup,Validators } from '@angular/forms';
import { TouristService } from '../tourist.service';
import { Club } from '../model/club.model';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';
import { Output,Input } from '@angular/core';

@Component({
  selector: 'xp-club-form',
  templateUrl: './club-form.component.html',
  styleUrls: ['./club-form.component.css']
})
export class ClubFormComponent implements OnChanges {

  @Output() clubUpdated = new EventEmitter<null>(); 
  @Input() club: Club;
  @Input() shouldEdit: boolean = false;


  clubForm = new FormGroup({

    name: new FormControl('',[Validators.required]),
    image: new FormControl(''), 
    description: new FormControl('') 

  });

  constructor(private service: TouristService, private authService: AuthService) {}

 
  ngOnChanges(): void {
    this.clubForm.reset();
    if(this.shouldEdit) {
      this.clubForm.patchValue(this.club);
    }
  }
 
  addClub(): void{
   
    const club: Club = { 
      id: 0, 
      name: this.clubForm.value.name || "", 
      description: this.clubForm.value.description || "",  
      image: this.clubForm.value.image ||"", 
      ownerId: this.authService.user$.value.id, 
      members: [] 
    } 
    
    this.service.addClub(club).subscribe({
      next: (_) => {
        this.clubUpdated.emit()
      }

    });

  } 
  updateClub():void{

      const club: Club = {
        id: this.club.id, 
        name: this.clubForm.value.name || "", 
        description: this.clubForm.value.description || "",  
        image: this.clubForm.value.image ||"", 
        ownerId: this.club.ownerId, 
        // memberIds: this.club.memberIds
        
      }
   
      
      this.service.updateClub(club).subscribe({
          next: (_) => {
            this.clubUpdated.emit()
          }
      })

  }
}
