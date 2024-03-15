import { Component, OnInit } from '@angular/core';
import { TourIssueService } from '../tour-issue.service';
import { ProfileService } from '../../administration/profile.service';
import { TourIssue } from '../model/tour-issue.model';
import { Tour } from '../../tour-authoring/model/tour.model';
import { RouteQuery } from 'src/app/shared/model/routeQuery.model';
import { RouteInfo } from 'src/app/shared/model/routeInfo.model';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { TourAuthoringService } from '../../tour-authoring/tour-authoring.service';
import { Profile } from '../../administration/model/profile.model';
import { PagedResult } from '../shared/model/paged-result.model';
import { TourIssueComment } from '../model/tour-issue-comment.model';
import { FormControl, FormGroup, Validators } from '@angular/forms';


@Component({
  selector: 'xp-tour-issue',
  templateUrl: './tour-issue.component.html',
  styleUrls: ['./tour-issue.component.css']
})
export class TourIssueComponent implements OnInit {
public constructor(private tourissueservice: TourIssueService, private profileservice:ProfileService, private router: Router, private route: ActivatedRoute){}

public selectedTourIssue : TourIssue
public selectedTour : Tour
public tourAuthor : Profile
public tourIssueAuthor : Profile
public routeQuery : RouteQuery;
public tourIssueId : Number;

tourIssueCommentForm = new FormGroup({
  comment: new FormControl('', Validators.required)
});

ngOnInit(): void {
  this.getTourIssueAgregate();
}

private getTourIssueAgregate() {
  this.route.paramMap.subscribe((params: ParamMap) => {
    this.tourIssueId = Number(params.get('id'));

    if (this.tourIssueId !== 0) {
      this.tourissueservice.getTourIssue(this.tourIssueId).subscribe((res: PagedResult<TourIssue>) => {
        this.selectedTourIssue = res.results[0];
        this.tourissueservice.getTour(parseInt(this.selectedTourIssue.tourId as string)).subscribe(result => { 
          this.selectedTour = result; 
          this.profileservice.getProfile(this.selectedTour.userId).subscribe(result => { this.tourAuthor = result; });
          for(let c of this.selectedTourIssue.comments as TourIssueComment[]){
            this.profileservice.getProfile(c.userId).subscribe(result => { 
              c.name = result.name; 
              c.surname = result.surname;
            });
          }
          
        });
        this.profileservice.getProfile(parseInt(this.selectedTourIssue.userId)).subscribe(result => { this.tourIssueAuthor = result; });
      });
    }
  });
}

addTourIssueComment(): void {
  const tourIssueComment : TourIssueComment = {
    tourIssueId: parseInt(this.selectedTourIssue.id as string),
    userId: -1,
    comment: this.tourIssueCommentForm.value.comment as string,
    creationDateTime: new Date().toISOString()
  }

  this.clearFormFields();

  this.tourissueservice.addTourIssueComment(tourIssueComment).subscribe({
    next: (_) => {
      this.getTourIssueAgregate();
    }
  });
}

resolveTourIssue(): void {
  this.selectedTourIssue.isResolved = true;
  this.selectedTourIssue.resolveDateTime = new Date();
  this.tourissueservice.resolveTourIssue(this.selectedTourIssue).subscribe({
    next: (_) => {
      this.getTourIssueAgregate();
    }
  });
}

  clearFormFields(): void {
    this.tourIssueCommentForm.get('comment')?.setValue('');
  }
}
