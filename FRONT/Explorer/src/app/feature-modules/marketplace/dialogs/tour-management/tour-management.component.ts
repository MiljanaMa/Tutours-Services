import {
  Component,
  EventEmitter,
  Inject,
  Input,
  OnInit,
  Output,
} from '@angular/core';
import { Tour } from '../../../tour-authoring/model/tour.model';
import { MarketplaceService } from '../../marketplace.service';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Discount } from '../../model/discount.model';
import { catchError, throwError } from 'rxjs';

@Component({
  selector: 'xp-tour-management',
  templateUrl: './tour-management.component.html',
  styleUrls: ['./tour-management.component.css'],
})
export class TourManagementComponent implements OnInit {
  publishedTours: Tour[] = [];
  discountedTours: Tour[] = [];
  allDiscounted: number[] = [];
  @Input() selectedTours: Tour[];

  displayedColumns: string[] = [
    'name',
    'description',
    'price',
    'add',
    'remove',
  ];

  constructor(
    @Inject(MAT_DIALOG_DATA) private discount: Discount,
    private marketplaceService: MarketplaceService,
  ) {}

  ngOnInit() {
    this.loadPublishedTours();
  }

  loadPublishedTours() {
    this.marketplaceService.getPublishedByAuthor().subscribe((result) => {
      this.publishedTours = result.results;
      this.loadAllDiscounted();
    });
  }

  loadAllDiscounted() {
    this.marketplaceService
      .getDiscountedTours(this.discount.id || 0)
      .subscribe((result) => {
        this.allDiscounted = result;
        this.removeToursFromAllDiscounted();
        this.removeToursBeforeDiscountEndDate();
        for (let td of this.discount.tourDiscounts || []) {
          this.discountedTours.push(
            this.publishedTours.filter((t) => t.id === td.tourId)[0],
          );
        }
      });
  }

  removeToursFromAllDiscounted() {
    this.publishedTours = this.publishedTours.filter((tour) => {
      const isInAllDiscounted = this.allDiscounted.includes(tour.id || 0);
      return !isInAllDiscounted;
    });
  }

  removeToursBeforeDiscountEndDate() {
    const twoWeeksAfterEndDate = new Date(this.discount.endDate);
    twoWeeksAfterEndDate.setDate(twoWeeksAfterEndDate.getDate() + 14);

    this.publishedTours = this.publishedTours.filter((tour) => {
      return !(
        tour.statusUpdateTime &&
        new Date(tour.statusUpdateTime) < twoWeeksAfterEndDate
      );
    });
  }

  isTourOnDiscount(tour: Tour): boolean {
    return this.discountedTours.some((t) => t.id === tour.id);
  }

  addTourToDiscount(tour: Tour): void {
    let tourDiscount = {
      discountId: this.discount.id || 0,
      tourId: tour.id || 0,
    };

    this.marketplaceService
      .addTourToDiscount(tourDiscount)
      .pipe(
        catchError((error) => {
          if (error.status === 400) {
            alert('Error: ' + error.message);
          }
          return throwError(error);
        }),
      )
      .subscribe((result) => {
        this.discountedTours.push(tour);
        this.discount.tourDiscounts?.push(tourDiscount);
      });
  }

  removeTourFromDiscount(tour: Tour): void {
    this.marketplaceService
      .removeTourFromDiscount(tour.id || 0)
      .subscribe((result) => {
        this.discountedTours.splice(this.discountedTours.indexOf(tour), 1);
        const tourDiscountIndex = this.discount.tourDiscounts?.findIndex(
          (td) => td.tourId === tour.id,
        );
        if (tourDiscountIndex !== -1) {
          this.discount.tourDiscounts?.splice(tourDiscountIndex || 0, 1);
        }
      });
  }
}
