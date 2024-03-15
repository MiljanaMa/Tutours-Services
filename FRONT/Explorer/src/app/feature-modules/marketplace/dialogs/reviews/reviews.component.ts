import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { TourReview } from 'src/app/feature-modules/tour-execution/model/tour-review.model';
import { MarketplaceService } from 'src/app/feature-modules/marketplace/marketplace.service';
import { animate, style, transition, trigger } from '@angular/animations';

@Component({
  selector: 'xp-reviews',
  templateUrl: './reviews.component.html',
  styleUrls: ['./reviews.component.css'],
  animations: [
    trigger('slideInOut', [
      transition(':enter', [
        style({ transform: 'translateX(100%)' }),
        animate('200ms ease-in', style({ transform: 'translateX(0%)' }))
      ]),
      transition(':leave', [
        animate('200ms ease-out', style({ transform: 'translateX(100%)' }))
      ])
    ])
  ]
})
export class ReviewsComponent implements OnInit {
  public tourReviews: TourReview[] = [];
  averageRate: number;

  constructor(@Inject(MAT_DIALOG_DATA) public tourId: number, private marketplaceService: MarketplaceService) { }

  ngOnInit(): void {
    this.marketplaceService.getReviewsByTour(this.tourId).subscribe(reviews => {
      this.tourReviews = reviews.results;

      this.calculateAverageRate();
    });

  }

  parseToInt(str: string): number {
    return parseInt(str);
  }

  calculateAverageRate() {
    this.marketplaceService.calculateAverageRate(this.tourReviews).subscribe(
      (response) => {
        this.averageRate = response;
      },
      
    );
  }

}
