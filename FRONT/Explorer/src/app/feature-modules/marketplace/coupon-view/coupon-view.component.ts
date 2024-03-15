import { Component } from '@angular/core';
import { MarketplaceService } from '../marketplace.service';
import { Coupon } from '../model/coupon-model';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';
import { PagedResults } from 'src/app/shared/model/paged-results.model';
import { Router } from '@angular/router';

@Component({
  selector: 'xp-coupon-view',
  templateUrl: './coupon-view.component.html',
  styleUrls: ['./coupon-view.component.css']
})

export class CouponViewComponent {

  coupons: Coupon[] = [];
  loggedId: number;

  constructor(private marketplaceService: MarketplaceService, private authService: AuthService, private router: Router){
    
  }

  ngOnInit(): void {
    this.loggedId = this.authService.user$.value.id; 
    this.getCoupons(); 
  }

  getCoupons(): void {
    this.marketplaceService.getCouponsForAuthor(this.loggedId).subscribe({
      next: (result: PagedResults<Coupon>) => {
        this.coupons = result.results; 
         
      },
      error:(err: any) => {
        console.log(err); 
      }
    })
  }

  deleteCoupon(couponId: number): void {
    this.marketplaceService.deleteCoupon(couponId).subscribe({
      next: () => {
        console.log("Coupon successfully deleted");
        this.getCoupons();
         
      },
      error:(err: any) => {
        console.log(err); 
      }
    })
  }

  updateCoupon(coupon: Coupon): void {
    this.router.navigate(['/coupon-create'], { queryParams: { couponData: JSON.stringify(coupon) } });
  }
  
  redirect(actionURL: string): void {
    if(actionURL){
      this.router.navigate([actionURL]);
    }
  }

}
