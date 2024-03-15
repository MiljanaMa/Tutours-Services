import { Component, OnInit } from '@angular/core';
import { AppRating } from '../model/app-rating.model';
import { AdministrationService } from '../administration.service';
import { PagedResult } from '../../tour-execution/shared/model/paged-result.model';

@Component({
  selector: 'xp-app-rating',
  templateUrl: './app-rating.component.html',
  styleUrls: ['./app-rating.component.css']
})
export class AppRatingComponent implements OnInit {

  ratings: AppRating[] = [];

  constructor(private service: AdministrationService) { }

  ngOnInit(): void {
    this.service.getAppRatings().subscribe({
      next: (result: PagedResult<AppRating>) => {
        this.ratings = result.results
      },
      error: (err: any) => {
        console.log(err)
      }
    })
  }

}
