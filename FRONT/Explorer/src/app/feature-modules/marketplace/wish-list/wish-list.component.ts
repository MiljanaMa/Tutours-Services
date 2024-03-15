import { Component, OnInit } from '@angular/core';
import { WishListItem } from '../model/wish-list-item.model';
import { WishList } from '../model/wish-list.model';
import { MarketplaceService } from '../marketplace.service';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';
import { PagedResult } from '../../tour-execution/shared/model/paged-result.model';
import { PagedResults } from 'src/app/shared/model/paged-results.model';
import { endWith } from 'rxjs';
import { Tour } from '../../tour-authoring/model/tour.model';

@Component({
  selector: 'xp-wish-list',
  templateUrl: './wish-list.component.html',
  styleUrls: ['./wish-list.component.css']
})
export class WishListComponent implements OnInit {

  wishListItems: WishListItem[] = [];
  wishListItemsAll: WishListItem[] = [];
  wishList: WishList;
  public tours: Tour[] = [];
  public wishedTours: Tour[] = [];

  constructor(private marketplaceService: MarketplaceService, private authService: AuthService){}

  ngOnInit(): void {
    this.getWishlistItems();
    this.getTours();
  }

  getTours(): void{
    this.marketplaceService.getPublishedTours().subscribe({
      next: (result: PagedResult<Tour>) => {
        this.tours = result.results;

        // filter tours by toursId u wishListItems.TourId
        this.wishedTours = this.tours.filter(tour =>
          this.wishListItems.some(item => item.tourId === tour.id)
        );
      }
    });

  }

  getWishlistItems(): void{
    this.marketplaceService.getWishListItemsForUser().subscribe({
      next: (value : PagedResults<WishListItem>) => {
        this.wishListItemsAll = value.results;
        console.log(this.wishListItems);
        console.log(value.results);

        const userId = this.authService.user$.value.id;
        this.wishListItems = this.wishListItemsAll.filter(item => item.userId === userId);

      },
      error:(err: any) => {
        console.log(err); 
      }
    })
    this.marketplaceService.getWishListForUser().subscribe({
      next: (result: WishList) => {
        this.wishList = result;
      },
      error:(err: any) => {
        console.log(err); 
      }
    })
  }
}
