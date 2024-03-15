import { ChangeDetectorRef, Component, Output } from '@angular/core';
import { Tour } from 'src/app/feature-modules/tour-authoring/model/tour.model';
import { MarketplaceService } from 'src/app/feature-modules/marketplace/marketplace.service';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';
import { User } from 'src/app/infrastructure/auth/model/user.model';
import { Router } from '@angular/router';
import { TourPurchaseToken } from '../../marketplace/model/tour-purchase-token.model';


@Component({
  selector: 'xp-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})

export class HomeComponent {
  public tours: Tour[] = [];
  public customTours: Tour[] = [];
  public isLoading = true;
  showOnDiscount: boolean = false;
  sortOrder: string = 'asc';
  public filteredTours: Tour[];
  public isOnDiscount: boolean = true;
  public filteredIds: number[] = []

  constructor(private marketplaceService: MarketplaceService, public authService: AuthService, public router: Router) { }

  ngOnInit(): void {
    if (this.router.url === '/home') {
      this.loadArchivedAndPublishedTours();
    } else if (this.router.url === '/purchased-tours') {
      this.loadPurchasedTours();
      this.loadCustomTours();
      console.log(this.tours);
    }
  }

  loadDiscounts(): void{
    this.marketplaceService.getToursOnDiscount().subscribe((ids => {
      this.filteredIds = ids;
    }))
  }

  loadArchivedAndPublishedTours(): void {
    this.marketplaceService.getArchivedAndPublishedTours().subscribe(tours => {
      this.tours = tours.results;
      this.isLoading = false;
    });
  }

  loadPurchasedTours(): void {
    this.marketplaceService.getPurchasedTours().subscribe(tokens => {
      tokens.results.forEach(token => {
        if (token.touristId === this.authService.user$.value.id) {
          this.addTourIfPurchased(token);
        }
      });
      this.isLoading = false;
    });
  }

  loadCustomTours(): void {
    this.marketplaceService.getCustomTours().subscribe(res => {
      this.customTours = res.results;
    });
  }

  addTourIfPurchased(token: any): void {
    this.marketplaceService.getPublishedTours().subscribe(tours => {
      tours.results.forEach(tour => {
        if (token.tourId === tour.id) {
          this.tours.push(tour);
        }
      });
    });
  }
  showPopup(): void {
    const popup = document.querySelector('.popup') as HTMLElement;

    if (popup) {
      popup.style.visibility = 'visible';
      popup.style.opacity = '1';

      setTimeout(() => {
        popup.style.visibility = 'hidden';
        popup.style.opacity = '0';
        window.location.href = 'https://www.youtube.com/watch?v=MMc8AP9KhEM';
      }, 2000);
    }
  }

  navigateToTourManagement(id: number): void{
    this.router.navigate(
      ['/custom-tour', id]
    );
  }

  navigateToCampaignCreation(id:number):void{
    this.router.navigate(
      ['/campaign/',id]);
  }

  filterTours() {
    if (this.showOnDiscount) {
      this.tours = this.tours.filter(tour => tour.id != null && this.filteredIds.includes(tour.id));
    } else {
      this.loadArchivedAndPublishedTours();
    }
  }

  sortTours() {
    this.filteredTours.sort((a, b) => {
      const discountA = (a.price - a.price) / a.price;
      const discountB = (b.price - b.price) / b.price;

      if (this.sortOrder === 'asc') {
        return discountA - discountB;
      } else {
        return discountB - discountA;
      }
    });
  }
}
