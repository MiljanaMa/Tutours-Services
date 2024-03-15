import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TourIssueService } from '../tour-issue.service';
import { TourIssue } from '../model/tour-issue.model';
import { PagedResult } from '../shared/model/paged-result.model';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';
import { Router } from '@angular/router';
import { Tour } from '../../tour-authoring/model/tour.model';
import { TourAuthoringService } from '../../tour-authoring/tour-authoring.service';
import { Observable } from 'rxjs';
import { PagedResults } from 'src/app/shared/model/paged-results.model';

@Component({
  selector: 'xp-tour-issue-tourist',
  templateUrl: './tour-issue-tourist.component.html',
  styleUrls: ['./tour-issue-tourist.component.css']
})
export class TourIssueTouristComponent implements OnChanges {
  tourIssues: TourIssue[] = [];
  selectedTourIssue: TourIssue;
  tours: Tour[];
  selectedTour : Tour;
  selectedTourID? : number | null;

  tourIssueForm = new FormGroup({
    category: new FormControl('', Validators.required),
    priority: new FormControl(''),
    description: new FormControl('', Validators.required)
  });

  user: any = this.authservice.user$;

  constructor(private tourIssueService: TourIssueService, private tourService: TourAuthoringService, private authservice: AuthService, private router: Router) {
    if(this.authservice.user$.value.role !== 'tourist') {
      this.router.navigate(['home']);
    }
  }

  ngOnInit(): void {
    this.tourIssueService.getTourIssueByUserId(this.user.value.id).subscribe({
      next: (result: PagedResult<TourIssue>) => {
        this.tourIssues = result.results;
      },
      error: (err: any) => {
        console.log(err);
      }
    })
    this.getTours();
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.tourIssueForm.patchValue(this.selectedTourIssue);
  }

  getTours(): void {
    this.tourService.getTours().subscribe({
      next: (result: PagedResults<Tour>) => {
        this.tours = result.results;
      },
      error: (err: any) => {
        console.log(err)
      }
    })
  }

  radioClicked(tour : Tour): number | undefined {
    this.selectedTour = tour;
    this.selectedTourID = this.selectedTour.id;
    return this.selectedTourID;
  }

  addTourIssue(): void {
    const tourIssue : TourIssue = {
      category: this.tourIssueForm.value.category || "",
      priority: this.tourIssueForm.value.priority as string || "1",
      description: this.tourIssueForm.value.description || "",
      creationDateTime: new Date(new Date().toUTCString()),
      userId: this.user.value.id,
      tourId: this.radioClicked(this.selectedTour) as string | undefined,
      comments: []
    }

    this.tourIssueService.addTourIssue(tourIssue).subscribe({
      next: (_) => {
        this.clearFormFields();
        this.ngOnInit();
      }
    });
  }

  onUpdateClicked(tourIssue: TourIssue): void {
    this.selectedTourIssue = tourIssue;
    this.tourIssueForm.patchValue(tourIssue);
  }

  onDeleteClicked(tourIssue: TourIssue): void {
    this.tourIssueService.deleteTourIssue(Number(tourIssue.id)).subscribe({
      next: (_) => {
        this.ngOnInit();
      },
      error: (err: any) => {
        console.log(err);
      }
    })
  }

  onViewClicked(tourIssue: TourIssue): void {
    window.location.href = `tourissue/${tourIssue.id}`;
  }

  updateTourIssue(): void {
    const tourIssue : TourIssue = {
      id: this.selectedTourIssue.id,
      category: this.tourIssueForm.value.category as string,
      priority: this.tourIssueForm.value.priority as string,
      description: this.tourIssueForm.value.description as string,
      creationDateTime: new Date(new Date().toUTCString()),
      userId: this.user.value.id,
      tourId: this.selectedTourIssue.tourId,
      comments: []
    }

    this.clearFormFields();

    this.tourIssueService.updateTourIssue(tourIssue).subscribe({
      next: (_) => {
        this.ngOnInit();
      }
    });
  }

  clearFormFields(): void {
    this.tourIssueForm.get('category')?.setValue('');
    this.tourIssueForm.get('priority')?.setValue('');
    this.tourIssueForm.get('description')?.setValue('');
  }
}
