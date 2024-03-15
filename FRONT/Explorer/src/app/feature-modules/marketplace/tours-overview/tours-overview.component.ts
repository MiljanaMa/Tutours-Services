import { Component, OnInit } from '@angular/core';
import { Tour } from '../../tour-authoring/model/tour.model';
import { MarketplaceService } from '../marketplace.service';
import { PagedResult } from '../../tour-execution/shared/model/paged-result.model';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Keypoint } from '../../tour-authoring/model/keypoint.model';
import { MarkerPosition } from 'src/app/shared/model/markerPosition.model';
import { Category, Object } from '../../tour-authoring/model/object.model';

@Component({
  selector: 'xp-tours-overview',
  templateUrl: './tours-overview.component.html',
  styleUrls: ['./tours-overview.component.css']
})
export class ToursOverviewComponent implements OnInit {

  public tours: Tour[];
  public tourFilterForm: FormGroup;
  showForm: boolean = false;
  public pointsOfInterest: MarkerPosition[];
  private temporary: MarkerPosition[];
  public radius: number;

  constructor(private marketplaceService: MarketplaceService) {
    this.tourFilterForm = new FormGroup({
      latitude: new FormControl('', [Validators.required]),
      longitude: new FormControl('', [Validators.required]),
      filterRadius: new FormControl(1, [Validators.min(0)]),
    });

    this.tours = [];
    this.pointsOfInterest = [];
    this.radius = 1;
  }

  ngOnInit(): void {
    this.marketplaceService.getPublishedTours().subscribe((res: PagedResult<Tour>) => {
      this.tours = res.results;
    });

    this.tourFilterForm.controls['filterRadius'].valueChanges.subscribe(value => {
      this.updateMapRadius(value);
    })
  }

  handleMapClick(event: number[]): void {
    if (event.length >= 2) {
      const latitude = event[0];
      const longitude = event[1];

      this.tourFilterForm.patchValue({
        latitude: latitude,
        longitude: longitude
      });

      this.temporary = [];

      this.getPointsOfInterest();
    }
  }

  getFilteredTours() {
    const { latitude, longitude, filterRadius } = this.tourFilterForm.value;
    
    this.marketplaceService.getFilteredTours(0, 0, latitude, longitude, filterRadius)
      .subscribe((res: PagedResult<Tour>) => {
        this.tours = res.results;
      });
  }

  getPointsOfInterest(){
    const { latitude, longitude, filterRadius } = this.tourFilterForm.value;

    this.marketplaceService.getPublicObjects(0, 0, latitude, longitude, this.tourFilterForm.value.filterRadius || 1).subscribe({
      next: (result: PagedResult<Object>) => {
        console.log(result);
        result.results.forEach((obj) => {
          this.temporary.push({
            longitude: obj.longitude,
            latitude: obj.latitude,
            color: obj.category == Category.OTHER ? 'object' : obj.category.toString().toLowerCase(),
            title: obj.name
          })
        });

        this.marketplaceService.getPublicKeyPoints(0, 0, latitude, longitude, this.tourFilterForm.value.filterRadius || 1).subscribe({
          next: (result: PagedResult<Keypoint>) => {
            result.results.forEach((kp) => {
              this.temporary.push({
                longitude: kp.longitude,
                latitude: kp.latitude,
                color: 'red',
                title: kp.name
              })
            });
            this.pointsOfInterest = this.temporary; // forcing map refresh, since ngOnChanges won't trigger on array push/pop
          }
        })
      }
    })
  }

  toggleForm() {
    this.showForm = !this.showForm;
  }

  updateMapRadius(newValue: number) {
    this.radius = newValue; // bug, this refreshes the map and after this second map refresh won't happen just because (when we load objects in new radius)
    this.getPointsOfInterest();
  }
}
