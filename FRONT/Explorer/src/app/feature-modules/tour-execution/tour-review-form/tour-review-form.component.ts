import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TourExecutionService } from '../tour-execution.service';
import { TourReview } from '../model/tour-review.model';
import { TourReviewString } from '../model/tour-review-string.model';
import { Tour } from '../../tour-authoring/model/tour.model';
import { TourAuthoringService } from '../../tour-authoring/tour-authoring.service';
import { PagedResults } from 'src/app/shared/model/paged-results.model';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';

@Component({
  selector: 'xp-tour-review-form',
  templateUrl: './tour-review-form.component.html',
  styleUrls: ['./tour-review-form.component.css']
})
export class TourReviewFormComponent implements OnChanges {
  tours: Tour[];
  selectedTour : Tour;
  selectedTourID? : number | null;

  @Output() tourReviewUpdated = new EventEmitter<null>(); 
  @Input() tourReview: TourReview;
  @Input() shouldEdit: boolean = false;

  constructor(private service: TourExecutionService, private tourService: TourAuthoringService) {}

  ngOnChanges(changes: SimpleChanges): void {
    this.getTours();
    this.tourReviewForm.reset();
    if(this.shouldEdit) {
      console.log(this.tourReview)
      this.tourReview.visitDate = this.tourReview.visitDate.split('T')[0]
      this.tourReviewForm.patchValue(this.tourReview);
    }
  }

  tourReviewForm = new FormGroup({
    rating: new FormControl('', [Validators.required]),
    comment: new FormControl('', [Validators.required]),
    visitDate: new FormControl('', [Validators.required]),
    imageLinks: new FormControl('', [Validators.required])
  })

  addTourReview(): void {
  console.log(this.tourReviewForm.value.visitDate)
  
const dateString = this.tourReviewForm.value.visitDate;
let visitDate = new Date()
  if (dateString) {
    const parts = dateString.split('-');
    const year = parseInt(parts[0], 10);
    const month = parseInt(parts[1], 10) - 1; // Months are 0-indexed in JavaScript Date
    const day = parseInt(parts[2], 10);
    visitDate = new Date(Date.UTC(year, month, day));
  }
  const tourReview: TourReviewString = {
    rating: Number(this.tourReviewForm.value.rating),
    comment: this.tourReviewForm.value.comment || "",
    visitDate: visitDate,
    ratingDate: new Date(),//.toISOString(),
    imageLinks: this.tourReviewForm.value.imageLinks?.split('\n') as string[],
    tourId: this.radioClicked(this.selectedTour) as string | undefined,
    userId: localStorage.getItem('loggedId')??'1'
  }
  this.clearFormFields();
  this.service.addTourReview(tourReview).subscribe({
    next: (tourReview) => {
      this.tourReviewUpdated.emit();
      alert('Successfully added tour review!');
    },
    error: (error) => {
      // Handle any errors that occur during the HTTP request
      console.error('Error adding tour review:', error);
    }
  });
  }

  updateTourReview(): void {
    const tourReview: TourReviewString = {
      rating: Number(this.tourReviewForm.value.rating),
      comment: this.tourReviewForm.value.comment || "",
      visitDate: new Date(/*this.tourReviewForm.value.visitDate*/), //+ "T00:00:00.000Z",
      ratingDate: new Date(),//.toISOString(),
      imageLinks: this.tourReviewForm.value.imageLinks as unknown as string[],
      tourId: this.tourReview.tourId.toString(),
      userId: localStorage.getItem('loggedId')??'1'
    }

    tourReview.id = this.tourReview.id;
    this.clearFormFields();

    this.service.updateTourReview(tourReview).subscribe({
      next: (_) => {
        this.tourReviewUpdated.emit();
        alert('Successfully updated tour review!');
      }
    });
  }

  clearFormFields(): void {
    this.tourReviewForm.value.rating = "";
    this.tourReviewForm.value.comment = "";
    this.tourReviewForm.value.visitDate = "";
    this.tourReviewForm.value.imageLinks = "";
  }

  getTours(): void {
    this.tourService.getTours().subscribe({
      next: (result: PagedResults<Tour>) => {
        this.tours = result.results;
      },
      error: (err: any) => {
        console.log(err)
      }
    })
  }

  radioClicked(tour : Tour): number | undefined {
    this.selectedTour = tour;
    this.selectedTourID = this.selectedTour.id;
    return this.selectedTourID;
  }

}
