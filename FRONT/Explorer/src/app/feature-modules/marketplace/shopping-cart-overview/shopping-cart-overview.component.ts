import { Component, OnInit } from '@angular/core';
import { Tour } from '../../tour-authoring/model/tour.model';
import { MarketplaceService } from '../marketplace.service';
import { PagedResult } from '../../tour-execution/shared/model/paged-result.model';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';
import { OrderItem } from '../model/order-item.model';
import { PagedResults } from 'src/app/shared/model/paged-results.model';
import { ShoppingCart } from '../model/shopping-cart.model';
import { Wallet } from '../model/wallet.model';
import { mergeMap } from 'rxjs';
import { Coupon } from '../model/coupon-model';
import {Discount} from "../model/discount.model";

@Component({
  selector: 'xp-shopping-cart-overview',
  templateUrl: './shopping-cart-overview.component.html',
  styleUrls: ['./shopping-cart-overview.component.css']
})
export class ShoppingCartOverviewComponent implements OnInit {

  orders: OrderItem[] =  [];
  loggedId: number;
  shoppingCart: ShoppingCart;
  totalPrice: number;
  wallet: Wallet;
  discounts: Discount[] = [];
  coupons: Coupon[][] = []
  selectedCoupons: any = {};
  couponsWithDiscount : Coupon [][] = [];
  originalPrices: { [key: string]: number } = {};
  selectedCouponObjects: Coupon[] = [];


  constructor(private marketplaceService: MarketplaceService, private authService: AuthService){}

  ngOnInit(): void {
    this.getOrders();
    this.getDiscounts();
    this.getWallet();
    this.loggedId = this.authService.user$.value.id;
  }
  getOrders(): void{
    this.marketplaceService.getOrdersForUser().subscribe({
      next: (result:PagedResults<OrderItem>) => {
        this.orders = result.results;
        for(let order of this.orders){
          this.marketplaceService.getCouponsForTourAndTourist(order.tourId!, this.loggedId).subscribe({
            next: (result:PagedResults<Coupon>) => {
              this.coupons.push(result.results);
              this.couponsWithDiscount.push(result.results);
              this.originalPrices[order.id] = order.tourPrice;
            },
            error:(err: any) => {
              console.log(err);
            }
          })
        }
      },
      error:(err: any) => {
        console.log(err);
      }
    })

    this.marketplaceService.getShoppingCartForUser().subscribe({
      next: (result:ShoppingCart) => {
        this.shoppingCart = result;

      },
      error:(err: any) => {
        console.log(err);
      }
    })
  }
  getDiscounts(): void {
    this.marketplaceService.getAllDiscounts().subscribe(result => {
      this.discounts = result.results;
    });
  }

  isOnDiscount(order: OrderItem): boolean {
    let isDiscount = false;

    this.discounts.forEach(discount => {
      discount.tourDiscounts?.forEach(tourDiscount => {
        if (tourDiscount.tourId === order.tourId) {
          isDiscount = true;
        }
      });
    });

    return isDiscount;
  }

  calculateNewPrice(order: OrderItem): number {
    let newPrice = order.tourPrice;

    this.discounts.forEach(discount => {
      discount.tourDiscounts?.forEach(tourDiscount => {
        if (tourDiscount.tourId === order.tourId) {
          newPrice -= newPrice * (discount.percentage / 100);
        }
      });
    });

    return newPrice;
  }


  selectCoupon(i:any, selectedValue: any, orderItemTourId: number): void{

    this.selectedCoupons[i] = selectedValue;

    const selectedCoupon = this.couponsWithDiscount[i].find(coupon => coupon.id === selectedValue);

    this.orders[i].tourPrice = this.originalPrices[this.orders[i].id];

    if (selectedCoupon) {
      const discount = selectedCoupon.discount;
      const originalTourPrice = this.orders[i].tourPrice;

      this.orders[i].tourPrice = originalTourPrice - (originalTourPrice * (discount / 100));

      const order = this.orders.find(order => order.tourId === orderItemTourId);
      if (order) {
        selectedCoupon.tourId = order.tourId;
      }

      const existingCouponIndex = this.selectedCouponObjects.findIndex(c => c.id === selectedCoupon.id);

      if (existingCouponIndex !== -1) {
        this.selectedCouponObjects[existingCouponIndex] = selectedCoupon;
      } else {
        this.selectedCouponObjects.push(selectedCoupon);
      }

    }
  }

  isCouponSelected(couponId: any): boolean {
    return  Object.values(this.selectedCoupons).includes(couponId);
  }

  calculateTotalPrice(): number {
    this.totalPrice = this.orders.reduce((total, order) => {
      let price = this.isOnDiscount(order) ? this.calculateNewPrice(order) : order.tourPrice;
      return total + price;
    }, 0);
  console.log(this.totalPrice)
    return this.totalPrice;
  }

  checkout(): void {
    this.getOrders();
    this.calculateTotalPrice();

    if (this.wallet.adventureCoins < this.totalPrice) {
      console.log(this.wallet);
      console.log('ukupna cena: ', this.totalPrice);
      alert('Error. You dont have enough ACs(adventure coins) in your wallet.');
    } else {
      if (window.confirm('Are you sure that you want to purchase these tours?')) {
        this.marketplaceService.buyShoppingCart(Number(this.shoppingCart.id), this.selectedCouponObjects).pipe(
        ).subscribe({
          next: (_) => {
            window.alert('Successfully purchased');
            this.getOrders();
          },
          error: (err: any) => {
            console.log(err);
            alert('Error. Some tours were already purchased.');
          }
        });
      }
    }
  }


  getWallet(): void {
    this.marketplaceService.getWalletForUser().subscribe({
      next:(result:Wallet)=>{
        this.wallet=result;
      },
      error:(err:any)=>{
        console.log(err);
      }
  });
  }



}
