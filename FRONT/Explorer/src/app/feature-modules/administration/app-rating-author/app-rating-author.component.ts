import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AdministrationService } from '../administration.service';
import { AppRating } from '../model/app-rating.model';

@Component({
  selector: 'xp-app-rating-author',
  templateUrl: './app-rating-author.component.html',
  styleUrls: ['./app-rating-author.component.css']
})
export class AppRatingAuthorComponent implements OnInit{
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
    this.service.getAppRatingForAuthor().subscribe({
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
      this.service.deleteAppRatingForAuthor().subscribe({
        next: () => { this.getAppRating(); }
      })
    }
  }
 
}
