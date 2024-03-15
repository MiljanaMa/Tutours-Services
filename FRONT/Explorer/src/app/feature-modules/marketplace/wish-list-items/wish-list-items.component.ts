import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { WishListItem } from '../model/wish-list-item.model';
import { MarketplaceService } from '../marketplace.service';
import { Tour } from '../../tour-authoring/model/tour.model';
import { Keypoint } from '../../tour-authoring/model/keypoint.model';
import { OrderItem } from '../model/order-item.model';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';
import { CartSuccessComponent } from '../dialogs/cart-success/cart-success.component';
import { CartWarningComponent } from '../dialogs/cart-warning/cart-warning.component';
import { MatDialog } from '@angular/material/dialog';


@Component({
  selector: 'xp-wish-list-items',
  templateUrl: './wish-list-items.component.html',
  styleUrls: ['./wish-list-items.component.css']
})
export class WishListItemsComponent implements OnInit {
  @Output() orderUpdated = new EventEmitter<null>();
  @Input() item: WishListItem;
  public tour: Tour;
  public firstKp: Keypoint;
  private lastOrderId: number;

  constructor(private dialog: MatDialog, private marketplaceService : MarketplaceService, private authService: AuthService){}

  async ngOnInit(): Promise<void> {
    await this.getTour();
    this.lastOrderId = 0;
    this.tour.keypoints = this.tour.keypoints?.sort((kp1, kp2) => {
      return (kp1.position || 0) - (kp2.position || 0);
    });
    if (this.tour.keypoints) {
      this.firstKp = this.tour.keypoints[0];
    }
  }

  getTour(): void{
    this.marketplaceService.getTourById(this.item.tourId).subscribe({
      next: (result: Tour) => {
        this.tour = result;
      },
      error:(err: any) => {
        console.log(err); 
      }
    })
  }

  addToCart(): void {
   this.lastOrderId = Date.now();
    this.marketplaceService.getAllOrders().subscribe((orders) => {
      if (orders.results && orders.results.length > 0) {
        const lastOrder = orders.results[orders.results.length - 1];
        this.lastOrderId = lastOrder.id + 1;
      } else {

        this.lastOrderId = 1;
      }


      const orderItem: OrderItem = {
        id: this.lastOrderId,
        tourId: this.tour.id,
        userId: this.authService.user$.value.id,
        tourName: this.tour.name,
        tourDescription: this.tour.description,
        tourPrice: this.tour.price
      };


      this.marketplaceService.addOrderItem(orderItem).subscribe({
        next: (_) => {
          this.dialog.open(CartSuccessComponent, { panelClass: 'success-dialog-container' });
          this.orderUpdated.emit();
        },
        error: (error) => {
          if (error.status === 409) {
            this.dialog.open(CartWarningComponent, { panelClass: 'warning-dialog-container' });
          }
        }
      });
    });
  }
}
