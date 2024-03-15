import { Component, OnInit } from '@angular/core';
import { MarketplaceService } from '../marketplace.service';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';
import { Coupon } from '../model/coupon-model';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Tourist } from '../model/tourist-model';
import { PagedResults } from 'src/app/shared/model/paged-results.model';
import { Tour } from '../../tour-authoring/model/tour.model';
import { ActivatedRoute } from '@angular/router';


@Component({
  selector: 'xp-coupon-create',
  templateUrl: './coupon-create.component.html',
  styleUrls: ['./coupon-create.component.css']
})
export class CouponCreateComponent {
  public coupon: Coupon;
  public couponForm : FormGroup;
  tourists: Tourist[] =  []; 
  tours: Tour[] = [];
  loggedId: number; 
  startDate: Date = new Date();
  isForUpdate: boolean = false;
  couponCode: string;
  couponId : number;

  constructor(private marketplaceService: MarketplaceService, private authService: AuthService, private route: ActivatedRoute){
    this.coupon = {id: 0, code: '', discount: 0, tourId: 0, touristId:0, authorId: 0, expiryDate: new Date()}
    this.couponForm = new FormGroup({
      discount: new FormControl(1, Validators.min(1)),
      expiryDate: new FormControl('', [Validators.required, this.futureDateValidator]),
      tourId: new FormControl(null),
      touristId: new FormControl(null,[Validators.required]),
    });
  }

  futureDateValidator(control: FormControl): { [key: string]: boolean } | null {
    const selectedDate = new Date(control.value);
    const currentDate = new Date();
  
    if (selectedDate < currentDate) {
      return { 'pastDate': true };
    }
  
    return null;
  }

  ngOnInit(): void {
    this.loggedId = this.authService.user$.value?.id; 

    // Retrieve coupon data from query parameters
    const queryParams = this.route.snapshot.queryParams;
    const couponData = queryParams['couponData'];

    if (couponData) {
      // If there's coupon data, parse it and pre-fill the form
      const coupon = JSON.parse(couponData);
      this.couponForm.patchValue(coupon);
      this.isForUpdate = true;
      this.couponCode = coupon.code;
      this.couponId = coupon.id;
    }

    this.getTourists(); 
    this.getTours(); 
  }

  getTourists(): void{
    this.marketplaceService.getAllTourists().subscribe({
      next: (result: PagedResults<Tourist>) => {
        this.tourists = result.results; 
         
      },
      error:(err: any) => {
        console.log(err); 
      }
    })
    
  }

  getTours(): void{
    this.marketplaceService.getAllToursForAuthor(this.loggedId).subscribe({
      next: (result:PagedResults<Tour>) => {
        this.tours = result.results;
         
      },
      error:(err: any) => {
        console.log(err); 
      }
    })
    
  }

  createCoupon(): void{
    
    let coupon: Coupon = {
      id: this.coupon.id,
      code: '',
      discount: this.couponForm.value.discount,
      tourId: this.couponForm.value.tourId,
      touristId: this.couponForm.value.touristId,
      authorId: this.loggedId,
      expiryDate: this.couponForm.value.expiryDate
    };

    this.marketplaceService.createCoupon(coupon).subscribe({
      next: () => { 
        window.alert("You have successfuly created coupon");
 
      },
      error:(err: any) => {
        console.log(err); 
      }
    })
    
  }

  updateCoupon() : void{
    let coupon: Coupon = {
      id: this.couponId,
      code: this.couponCode,
      discount: this.couponForm.value.discount,
      tourId: this.couponForm.value.tourId,
      touristId: this.couponForm.value.touristId,
      authorId: this.loggedId,
      expiryDate: this.couponForm.value.expiryDate
    };

    this.marketplaceService.editCoupon(coupon).subscribe({
      next: () => { 
        window.alert("You have successfuly edited coupon");
 
      },
      error:(err: any) => {
        console.log(err); 
      }
    })
  }
}
