import { Component, OnInit } from '@angular/core';
import { Club } from '../model/club.model';
import { TouristService } from '../tourist.service';
import { PagedResults } from 'src/app/shared/model/paged-results.model';

@Component({
  selector: 'xp-clubs-ranking',
  templateUrl: './clubs-ranking.component.html',
  styleUrls: ['./clubs-ranking.component.css']
})
export class ClubsRankingComponent implements OnInit{

  public clubs: Club[] =  [];
  public sortOrder: string = 'wins';  

  constructor(private service: TouristService) {}

  ngOnInit(): void {
    this.service.updateFights().subscribe({
      next: () => {
        this.getClubs(); 
      },
      error: () => {
        this.getClubs(); 
      }
    });
  }
  
  getClubs(): void{

    this.service.getClubsUpdatedModel().subscribe({
      next:(result:PagedResults<Club>) => {
        this.clubs = result.results;
      },
      error:(err: any) => {
        console.log(err); 
      }
    })
  }
  getXP(club: Club): number{
    return club?.members?.reduce((sum, person) => sum + (person?.xp || 0), 0) || 0;
  }
  updateSortOrder(value: string): void {
    this.sortOrder = value;
    if (value === 'wins') {
      this.clubs = this.clubs.sort((a, b) => ((b.fightsWon || 0)- (a.fightsWon || 0)));
    } else {
       this.clubs = this.clubs.sort((a, b) => this.getXP(b) - this.getXP(a));
    }
  }

}
