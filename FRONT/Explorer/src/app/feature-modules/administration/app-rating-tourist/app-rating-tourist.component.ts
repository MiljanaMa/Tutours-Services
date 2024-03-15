import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AdministrationService } from '../administration.service';
import { AppRating } from '../model/app-rating.model';

@Component({
  selector: 'xp-app-rating-tourist',
  templateUrl: './app-rating-tourist.component.html',
  styleUrls: ['./app-rating-tourist.component.css']
})
export class AppRatingTouristComponent implements OnInit{

  appRating: AppRating;
  mode: string = 'preview';
  appRatingExist: boolean = false;
  showForm: boolean = false;

  appRatingIdForm = new FormGroup({
    id: new FormControl(1, [Validators.required])
  });

  constructor(private service: AdministrationService) { }

  ngOnInit(): void {
    this.getAppRating();
  }

  getAppRating(): void{ 
    this.showForm = false;
    this.mode = 'preview';
    this.service.getAppRatingForTourist().subscribe({
      next: (result: AppRating) => 
      { 
        this.appRating = result; 
        this.appRatingExist = true;
      },
      error: () => {
        this.appRatingExist = false;
        this.showForm = false;
      }
    })
  }
  
  addAppRating(): void{
    this.mode = 'add';
    this.showForm = true;
  }

  editAppRating(): void{
    this.mode = 'edit';
    this.showForm = true;
  }

  deleteAppRating(): void{
    if(this.appRating){
      this.service.deleteAppRatingForTourist().subscribe({
        next: () => { this.getAppRating(); }
      })
    }
  }
 
}
