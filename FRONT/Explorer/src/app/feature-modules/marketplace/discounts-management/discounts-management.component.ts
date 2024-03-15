import {
  Component,
  EventEmitter,
  Input,
  Output,
  ViewChild,
} from '@angular/core';
import { RouteInfo } from '../../../shared/model/routeInfo.model';
import { RouteQuery } from '../../../shared/model/routeQuery.model';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Discount } from '../model/discount.model';
import { MarketplaceService } from '../marketplace.service';
import { Tour } from '../../tour-authoring/model/tour.model';
import { TourAuthoringService } from '../../tour-authoring/tour-authoring.service';
import { TourDiscount } from '../model/tour-discount.model';
import { MatDialog } from '@angular/material/dialog';
import { TourManagementComponent } from '../dialogs/tour-management/tour-management.component';

@Component({
  selector: 'xp-discounts-management',
  templateUrl: './discounts-management.component.html',
  styleUrls: ['./discounts-management.component.css'],
})
export class DiscountsManagementComponent {
  @Output() discountUpdated = new EventEmitter<Discount>();
  @Output() routeFound = new EventEmitter<RouteInfo>();

  @Input() tourId: number;
  @Input() routeQuery: RouteQuery;
  @Input() selectedDiscount: Discount;
  @Input() discountsCount: number = 0;
  @Input() mode: string = 'add';
  public tours: Tour[];
  public discounts: Discount[] = [];
  public discountForm: FormGroup;
  public availableTours: Tour[];
  public selectedTours: Tour[] = [];
  displayedColumns: string[] = [
    'startDate',
    'endDate',
    'percentage',
    'edit',
    'removeDiscount',
    'manageTours',
  ];
  showForm: boolean = false;
  tourDiscount: TourDiscount;
  isEditing: boolean = false;
  tourStatusUpdateTime: Date;
  showTours: boolean = false;

  constructor(
    private marketplaceService: MarketplaceService,
    private tourAuthoringService: TourAuthoringService,
    private dialog: MatDialog,
  ) {
    this.discountForm = new FormGroup({
      startDate: new FormControl('', [Validators.required]),
      endDate: new FormControl('', [Validators.required]),
      percentage: new FormControl(0, [
        Validators.min(0),
        Validators.max(100),
        Validators.required,
      ]),
    });
    this.tourDiscount = {} as TourDiscount;
  }

  ngOnInit(): void {
    this.marketplaceService.getDiscountsByAuthor().subscribe((result) => {
      this.discounts = result.results;
    });
  }

  ngOnChanges(): void {
    this.discountForm.reset();
    if (this.mode === 'edit') {
      this.discountForm.patchValue(this.selectedDiscount);
    }
  }

  addDiscount(): void {
    if (!this.discountForm.valid) {
      window.alert('Form invalid');
    } else {
      let discount: Discount = {
        startDate:
          this.discountForm.value.endDate.toISOString().split('T')[0] || '',
        endDate:
          this.discountForm.value.startDate.toISOString().split('T')[0] || '',
        percentage: this.discountForm.value.percentage || 0,
      };

      if (this.mode === 'add') {
        this.marketplaceService.addDiscount(discount).subscribe({
          next: () => {
            this.discountUpdated.emit();
            this.getDiscountsForAuthor();
            this.isEditing = false;
            this.resetForm();
          },
          error: (error) => {
            if (error.error && error.error.message) {
              window.alert(error.error.message);
            } else {
              window.alert(
                'An error occurred while adding the tour to the discount. Please try again.',
              );
            }
          },
        });
      } else if (this.mode === 'edit') {
        discount.id = this.selectedDiscount.id;
        this.marketplaceService.updateDiscount(discount).subscribe({
          next: (result) => {
            this.marketplaceService
              .getDiscountsByAuthor()
              .subscribe((result) => {
                this.discounts = result.results;
                this.isEditing = false;
              });
            window.alert(`You have successfully updated discount`);
            this.discountUpdated.emit();
            this.resetForm();
          },
          error: (error) => {},
        });
      }
    }
  }

  resetForm(): void {
    this.showForm = false;
    this.isEditing = false;
    this.discountForm.reset();
    this.discountForm.clearValidators();
  }

  getDiscountsForAuthor() {
    this.marketplaceService.getDiscountsByAuthor().subscribe((discounts) => {
      this.discounts = discounts.results;
    });
  }

  removeDiscount(discount: Discount): void {
    this.marketplaceService.removeDiscount(discount.id || 0).subscribe(() => {
      this.getDiscountsForAuthor();
    });
  }

  toggleTourManagement(discount: Discount): void {
    this.dialog.open(TourManagementComponent, { data: discount });
  }

  editDiscount(discount: Discount): void {
    this.selectedDiscount = discount;
    this.isEditing = true;
    this.mode = 'edit';
    this.showForm = true;
    this.discountForm.patchValue({
      startDate: new Date(this.selectedDiscount.startDate),
      endDate: new Date(this.selectedDiscount.endDate),
      percentage: this.selectedDiscount.percentage,
    });
  }

  updateDiscount() {
    const formValue = this.discountForm.value;

    this.selectedDiscount = {
      ...this.selectedDiscount,
      startDate: formValue.startDate.toISOString().split('T')[0],
      endDate: formValue.endDate.toISOString().split('T')[0],
      percentage: formValue.percentage,
    };

    this.marketplaceService
      .updateDiscount(this.selectedDiscount)
      .subscribe((result) => {
        this.selectedDiscount = result;
        this.marketplaceService
          .getDiscountsByAuthor()
          .subscribe((updatedDiscounts) => {
            this.discounts = updatedDiscounts.results;
            alert('Discount successfully updated.');
          });
      });
  }
}
