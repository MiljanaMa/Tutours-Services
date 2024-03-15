import { Component, OnInit } from '@angular/core';
import { TourIssueService } from '../tour-issue.service';
import { TourIssue } from '../model/tour-issue.model';
import { PagedResult } from '../shared/model/paged-result.model';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';
import { Router } from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Status, Tour } from '../../tour-authoring/model/tour.model';

@Component({
  selector: 'xp-tour-issue-admin',
  templateUrl: './tour-issue-admin.component.html',
  styleUrls: ['./tour-issue-admin.component.css']
})
export class TourIssueAdminComponent implements OnInit {
  constructor(private service: TourIssueService, private authservice: AuthService, private router: Router) {
    if(this.authservice.user$.value.role !== 'administrator') {
      this.router.navigate(['home']);
    }
  }

  tourIssues: TourIssue[] = [];
  selectedTourIssue: TourIssue;
  td = new Date().getTime();
  
  ngOnInit(): void {
    this.service.getTourIssues().subscribe({
      next: (result: PagedResult<TourIssue>) => {
        this.tourIssues = result.results;
      },
      error: (err: any) => {
        console.log(err);
      }
    })
  }

  onChange(event: any, index: number) {
    const newResolveDate = event.target?.value;
    this.tourIssues[index].resolveDateTime = newResolveDate;
  }

  getTourIssues(): void {
    this.service.getTourIssues().subscribe({
      next: (result: PagedResult<TourIssue>) => {
        this.tourIssues = result.results;
      },
      error: (err: any) => {
        console.log(err);
      }
    })
  }

  calculateDifference(creationDate: Date): number {
    const today = new Date();
    const daysDifference = Math.floor((today.getTime() - new Date(creationDate).getTime()) / (1000 * 60 * 60 * 24));
    console.log(daysDifference);
    return daysDifference;
  }

  setResolveDateTime(tourIssue: TourIssue): void {
    this.selectedTourIssue = tourIssue;
    this.selectedTourIssue.resolveDateTime = new Date(tourIssue.resolveDateTime as Date);
    this.service.setResolveDateTime(tourIssue).subscribe({
      next: () => {
        this.ngOnInit();
      },
      error: (err: any) => {
        console.log(err);
      }
    });
  }

  disableTour(tourIssue: TourIssue): void {
    this.service.getTour(parseInt(tourIssue.tourId as string)).subscribe({
      next: (result: Tour) => {
        result.status = Status.DISABLED;
        this.service.updateTour(result).subscribe({
          next: () => {
            alert("Successfully disabled tour! ඞ");
          },
          error: (err: any) => {
            console.log(err);
          }
        });
      }
    })
  }

  public resolveButtonDisabled(tourIssue: TourIssue): boolean
  {
    if(tourIssue.isResolved === true)
      return true;
    
    if(tourIssue.resolveDateTime == undefined || tourIssue.resolveDateTime == null)
    {
      return true;
    }

    if(new Date(tourIssue.resolveDateTime).getTime() < new Date().getTime())
    {
      return false;
    }
  
    return true;
  }

  onViewClicked(tourIssue: TourIssue): void {
    window.location.href = `tourissue/${tourIssue.id}`;
  }
}
