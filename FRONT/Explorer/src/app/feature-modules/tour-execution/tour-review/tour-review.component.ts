import { Component, OnInit } from '@angular/core';
import { TourExecutionService } from '../tour-execution.service';
import { PagedResults } from 'src/app/shared/model/paged-results.model';
import { TourReview } from '../model/tour-review.model';
import { Tour } from '../../tour-authoring/model/tour.model';
import { TourAuthoringService } from '../../tour-authoring/tour-authoring.service';

@Component({
  selector: 'xp-tour-review',
  templateUrl: './tour-review.component.html',
  styleUrls: ['./tour-review.component.css']
})
export class TourReviewComponent implements OnInit {

  tourReview: TourReview[] = [];
  tours: Tour[];
  selectedTour : Tour;
  selectedTourID? : number | null;
  selectedTourReview: TourReview;
  shouldEdit: boolean;
  shouldRenderTourReviewForm: boolean = false;

  constructor(private service: TourExecutionService, private tourService: TourAuthoringService) {}

  ngOnInit(): void {
    this.getTourReviews();
    //.getTours();
  }

  getTourReviews(): void {
    this.service.getTourReviews().subscribe({
      next: (result: PagedResults<TourReview>) => {
        this.tourReview = result.results;
      },
      error: (err: any) => {
        console.log(err)
      }
    })
  }

  onEditClicked(tourReview: TourReview): void {
    this.shouldEdit = true;
    this.selectedTourReview = tourReview;
  }

  onAddClicked(): void {
    this.shouldRenderTourReviewForm = true;
    this.shouldEdit = false;
  }

  deleteTourReview(tourReview: TourReview): void {
    this.service.deleteTourReview(tourReview).subscribe({
      next: (_) => {
        this.getTourReviews();
      }
    })
  }

  /*getTours(): void {
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
  }*/
}
