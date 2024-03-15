import { Component, EventEmitter, Input, OnChanges, OnInit, Output } from '@angular/core';
import { AppRating } from '../model/app-rating.model';
import { AdministrationService } from '../administration.service';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';

function ratingValidator(control: AbstractControl){
  const rating = control.value;
  const valid = !isNaN(rating) && rating >= 1 && rating <= 5;
  return valid ? null : {invalidRating: true};
}

@Component({
  selector: 'xp-app-rating-form-tourist',
  templateUrl: './app-rating-form-tourist.component.html',
  styleUrls: ['./app-rating-form-tourist.component.css']
})


export class AppRatingFormTouristComponent implements OnChanges{

  @Output() appRatingUpdated = new EventEmitter<null>();
  @Input() appRating: AppRating;
  @Input() mode: string;

  constructor(private service: AdministrationService) { }

  ngOnChanges(): void{
    this.appRatingForm.reset();
    if(this.mode == 'edit'){
      this.appRatingForm.patchValue(this.appRating);
    }
  }

  appRatingForm = new FormGroup({
    rating: new FormControl(5, [Validators.required, ratingValidator]),
    comment: new FormControl(''),
  })

  saveAppRating(): void{
    this.appRating = {
      rating: this.appRatingForm.value.rating || 5,
      comment: this.appRatingForm.value.comment || "",
      lastModified: new Date()
    }

    if(this.mode == 'edit'){
      this.service.updateAppRatingForTourist(this.appRating).subscribe({
        next: () => { this.appRatingUpdated.emit() }
      });
    }
    else if(this.mode == 'add'){
      this.service.addAppRatingForTourist(this.appRating).subscribe({
        next: () => {  this.appRatingUpdated.emit() }
      });
    }
  }
}
