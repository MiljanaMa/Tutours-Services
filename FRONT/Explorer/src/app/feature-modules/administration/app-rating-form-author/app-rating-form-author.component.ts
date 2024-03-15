import { Component, EventEmitter, Input, OnChanges, OnInit, Output } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { AdministrationService } from '../administration.service';
import { AppRating } from '../model/app-rating.model';

function ratingValidator(control: AbstractControl){
  const rating = control.value;
  const valid = !isNaN(rating) && rating >= 1 && rating <= 5;
  return valid ? null : {invalidRating: true};
}

@Component({
  selector: 'xp-app-rating-form-author',
  templateUrl: './app-rating-form-author.component.html',
  styleUrls: ['./app-rating-form-author.component.css']
})
export class AppRatingFormAuthorComponent implements OnChanges {

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
      this.service.updateAppRatingForAuthor(this.appRating).subscribe({
        next: () => { this.appRatingUpdated.emit() }
      });
    }
    else if(this.mode == 'add'){
      this.service.addAppRatingForAuthor(this.appRating).subscribe({
        next: () => {  this.appRatingUpdated.emit() }
      });
    }
  }

}

