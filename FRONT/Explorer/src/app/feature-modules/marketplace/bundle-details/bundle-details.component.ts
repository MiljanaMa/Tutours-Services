import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MarketplaceService } from '../marketplace.service';
import { Bundle } from '../model/bundle.model';
import { Tour } from '../../tour-authoring/model/tour.model';
import { BundlePrice } from '../model/bundle-price.model';

@Component({
  selector: 'xp-bundle-details',
  templateUrl: './bundle-details.component.html',
  styleUrls: ['./bundle-details.component.css']
})
export class BundleDetailsComponent implements OnInit {
  bundleId!: number;
  bundle:Bundle;
  editing:boolean = false;
  tours: Tour[];
  toursByAuthor:Tour[];
  name:string
  newName:string = '';
  price:number;
  bundlePrice: BundlePrice;
  newPrice: number = 0;
  

  constructor(private route: ActivatedRoute, private service:MarketplaceService) {}

  ngOnInit(): void {
    // Retrieve bundleId and price from the route parameters
    const bundleIdParam = this.route.snapshot.paramMap.get('bundleId');
    this.bundleId = +[bundleIdParam]
    
    this.getBundle(this.bundleId);
    this.getPriceForBundle(this.bundleId);
    this.getToursByAuthor();
   // this.createPriceForBundle();
  }

  getPriceForBundle(id:number) {
    this.service.getPriceForBundle(id).subscribe(
      (result) => {
        this.bundlePrice = result;
      },
      (error) => {
        console.error('Error fetching bundle price:', error);
      }
    );
  }
  getBundle(id:number){
    this.service.getBundle(id).subscribe(
      (result) => {
        this.bundle = result;
      },
      (error) => {
        console.error('Error fetching bundle:', error);
      }
    );
  }
  
  toggleEditing() {
    this.editing = !this.editing;
  }

  saveName() {
    if (this.bundle && this.newName !== this.bundle.name) {
      this.bundle.name = this.newName;

      
      this.service.updateBundle().subscribe(
        (updatedBundle) => {
          console.log('Bundle name updated successfully:', updatedBundle);
          this.toggleEditing(); 
        },
        (error) => {
          console.error('Error updating bundle name:', error);
          
        }
      );
    } else {
      this.toggleEditing();
    }
  }
  getToursByAuthor(){
      this.service.getToursByAuthor().subscribe(
        (result)=>{
          this.toursByAuthor= result.results;
        },
        (error)=>{
          console.error('Error fethching tours', error);
        }
      )
    }

    addToBundle(tourId: number, bundleId: number) {
      this.service.addTourToBundle(tourId, bundleId).subscribe(
        (result) => {
          console.log('Tour added to bundle successfully:', result);
          // Refresh the tours in the bundle after adding a new one
          this.getBundle(bundleId);
  
          // Recalculate the bundle price after adding a tour
          this.service.calculateBundlePrice(bundleId).subscribe(
            (calculatedPrice) => {
              this.bundlePrice.totalPrice = calculatedPrice;
              // Handle the calculated price as needed
            },
            (error) => {
              console.error('Error calculating bundle price:', error);
            }
          );
        },
        (error) => {
          console.error('Error adding tour to bundle:', error);
        }
      );
    }
  
    deleteFromBundle(tourId: number, bundleId: number) {
      // Call your service method to add the tour to the bundle
      this.service.deleteTourFromBundle(tourId, bundleId).subscribe(
        (result) => {
          console.log('Tour added to bundle successfully:', result);
          // Refresh the tours in the bundle after adding a new one
          this.getBundle(bundleId);
          this.service.calculateBundlePrice(bundleId).subscribe(
            (calculatedPrice) => {
              this.bundlePrice.totalPrice = calculatedPrice;
              // Handle the calculated price as needed
            },
            (error) => {
              console.error('Error calculating bundle price:', error);
            }
          );
        },
        (error) => {
          console.error('Error adding tour to bundle:', error);
        }
      );
    }

    createBundlePrice() {
      // Skip creation if bundle is not defined
      if (!this.bundle || this.bundle.id === undefined) {
        return;
      }
  
      // Ensure that bundle.id is defined
      const bundleId: number = this.bundle.id;
  
      // Call the calculateBundlePrice method from the service to get an Observable<number>
      const totalPriceObservable = this.service.calculateBundlePrice(bundleId);
  
      // Subscribe to the observable to get the calculated total price
      totalPriceObservable.subscribe(
        (totalBundlePrice) => {
          // Create a new BundlePrice object with the calculated total price
          const newBundlePrice: BundlePrice = {
            bundleId: bundleId,
            totalPrice: totalBundlePrice,
          };
  
          // Create or update the BundlePrice
          this.service.createPriceForBundle(newBundlePrice).subscribe(
            (createdBundlePrice) => {
              console.log('Bundle price created/updated successfully:', createdBundlePrice);
              this.bundlePrice = createdBundlePrice;
  
              // After creating or updating the bundle price, you may not need to do anything else
            },
            (error) => {
              if (error.status === 409) {
                console.error('Duplicate key violation. The bundle price already exists.');
                // You can show a message to the user or handle it as needed
              } else {
                console.error('Error creating/updating bundle price:', error);
              }
            }
          );
        },
        (error) => {
          console.error('Error calculating bundle price:', error);
        }
      );
    }
}