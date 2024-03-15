import { ChangeDetectorRef, Component, Input, OnInit } from '@angular/core';
import { Tour } from '../../tour-authoring/model/tour.model';
import { Keypoint } from '../../tour-authoring/model/keypoint.model';
import { OrderItem } from '../model/order-item.model';
import { MarketplaceService } from '../marketplace.service';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';
import { ShoppingCartOverviewComponent } from '../shopping-cart-overview/shopping-cart-overview.component';
import { ShoppingCart } from '../model/shopping-cart.model';
import { Coupon } from '../model/coupon-model';
import { PagedResults } from 'src/app/shared/model/paged-results.model';
import {Discount} from "../model/discount.model";

@Component({
  selector: 'xp-shopping-cart',
  templateUrl: './shopping-cart.component.html',
  styleUrls: ['./shopping-cart.component.css']
})
export class ShoppingCartComponent implements OnInit {

  @Input() order: OrderItem;
  shoppingCart: ShoppingCartOverviewComponent;
  shoppingCartForUser: ShoppingCart;
  loggedId: number;
  coupons: Coupon[] = [];

  discounts: Discount[] = [];
  constructor(private marketplaceService: MarketplaceService, private authService: AuthService, private changeDetectorRef: ChangeDetectorRef){
  }

  ngOnInit(): void {
    this.getShoppingCart();
    this.loggedId = this.authService.user$.value.id;
    this.marketplaceService.getAllDiscounts().subscribe(result => {
      this.discounts = result.results;
      console.log(this.discounts);
    });
  }


  Delete(orderItem: OrderItem): void {
    this.marketplaceService.deleteOrderItem(Number(orderItem.id)).subscribe({
      next: (_) => {
        window.alert('Item deleted successfully');
      },
      error: (err: any) => {
        console.log(err);
        alert('An error occurred while deleting the order item. Please try again later.');
      }
    })
  }

  getShoppingCart(): void{
    this.marketplaceService.getShoppingCartForUser().subscribe({
      next: (result:ShoppingCart) => {
        this.shoppingCartForUser = result;

      },
      error:(err: any) => {
        console.log(err);
      }
    })
  }

  calculateNewPrice(order: OrderItem): number {
    let newPrice = 0;

    this.discounts.forEach(discount => {
      discount.tourDiscounts?.forEach(tourDiscount => {
        if (tourDiscount.tourId === order.tourId) {
          newPrice = order.tourPrice - (order.tourPrice * (discount.percentage / 100));
        }
      });
    });

    return newPrice;
  }

  isOnDiscount(order: OrderItem): boolean {
    let isDiscount = false;
    let newPrice = 0;

    this.discounts.forEach(discount => {
      discount.tourDiscounts?.forEach(tourDiscount => {
        if (tourDiscount.tourId === order.tourId) {
          isDiscount = true;
          newPrice = order.tourPrice - (order.tourPrice * (discount.percentage / 100));
          console.log(`The tour is on discount! The new price is: ${newPrice}`);
        }
      });
    });

    return isDiscount;
  }

  getDiscountPercentage(order: OrderItem): number {
    let discountPercentage = 0;

    this.discounts.forEach(discount => {
      discount.tourDiscounts?.forEach(tourDiscount => {
        if (tourDiscount.tourId === order.tourId) {
          discountPercentage = discount.percentage;
        }
      });
    });

    return discountPercentage;
  }
}
