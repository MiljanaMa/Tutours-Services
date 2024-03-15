import { Component } from '@angular/core';
import { MarketplaceService } from '../marketplace.service';
import { Bundle } from '../model/bundle.model';
import { Tour } from '../../tour-authoring/model/tour.model';

@Component({
  selector: 'xp-bundle-overview',
  templateUrl: './bundle-overview.component.html',
  styleUrls: ['./bundle-overview.component.css']
})
export class BundleOverviewComponent {
  bundles: Bundle[] = [];
  tours: Tour[]=[]

  constructor(private service: MarketplaceService) {}

  ngOnInit(): void {

    this.getAllBundles();
    //this.getToursByAuthor();
    // Call the service method to get all bundles
    
  }

  getAllBundles(){
    this.service.getAllBundles().subscribe(
      (result) => {
        this.bundles = result.results;
      },
      (error) => {
        console.error('Error fetching bundles:', error);
        // Handle the error, e.g., display an error message to the user
      }
    );
  }

  // getToursByAuthor(){
  //   this.service.getToursByAuthor().subscribe(
  //     (result)=>{
  //       this.tours= result.results;
  //     },
  //     (error)=>{
  //       console.error('Error fethching tours', error);
  //     }
  //   )
  // }

  addTourToBundle(tourId: number, bundleId: number) {
    

    this.service.addTourToBundle(tourId, bundleId).subscribe(
      (updatedBundle) => {
        console.log('Bundle updated successfully:', updatedBundle);
        // Optionally, you can do something with the updatedBundle if needed
      },
      (error) => {
        console.error('Error adding tour to bundle:', error);
        // Handle the error, e.g., display an error message to the user
      }
    );
  }

  
}
