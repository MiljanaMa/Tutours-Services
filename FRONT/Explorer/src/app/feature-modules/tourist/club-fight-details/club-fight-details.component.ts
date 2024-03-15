import { Component, OnInit } from '@angular/core';
import { ClubFight } from '../model/club-fight.model';
import { ClubFightXPInfo } from '../model/club-fight-xp-info.model';
import { TouristService } from '../tourist.service';
import { ActivatedRoute, ParamMap } from '@angular/router';

@Component({
  selector: 'xp-club-fight-details',
  templateUrl: './club-fight-details.component.html',
  styleUrls: ['./club-fight-details.component.css']
})
export class ClubFightDetailsComponent implements OnInit {

  public clubFight: ClubFight;
  public clubFightXPInfo: ClubFightXPInfo;
  public tillFightEnd: any;

  constructor(private touristService: TouristService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.touristService.updateFights().subscribe({
      next: () => {
        this.getFight();
      },
      error: () => {
        this.getFight();
      }
    });
  }

  getFight() {
    this.route.paramMap.subscribe((params: ParamMap) => {
      let fightId = Number(params.get('id'));

      if(fightId !== 0){
          this.touristService.getFightById(fightId).subscribe({ 
            next: (result: ClubFight) => { 
              this.clubFight = result;
              this.tillFightEnd = this.getDuration((new Date(this.clubFight.endOfFight)).valueOf() - (new Date()).valueOf()); // aaaaaaaaaaaaaaaaaaaa
              setInterval(() => {
                this.tillFightEnd = this.getDuration((new Date(this.clubFight.endOfFight)).valueOf() - (new Date()).valueOf()); 
              }, 5 * 1000); 
              this.getFightXPInfo(fightId);
            },
            error: () => { }
        });
      }
    });
  }

  getFightXPInfo(fightId: number) {
    this.touristService.getFightXPInfo(fightId).subscribe({ 
      next: (result: ClubFightXPInfo) => { 
        this.clubFightXPInfo = result;
      },
      error: () => { }
    });
  }

  getDuration(milli: number) {
    let days = Math.floor(milli/(86400 * 1000));
    milli -= days*(86400*1000);
    let hours = Math.floor(milli/(60 * 60 * 1000 ));
    milli -= hours * (60 * 60 * 1000);
    let minutes = Math.floor(milli/(60 * 1000));
  
    return ({'days': days, "hours": hours, "minutes": minutes})
  };

  endFight(): void{
    this.route.paramMap.subscribe((params: ParamMap) => {
      let fightId = Number(params.get('id'));
      this.touristService.endFight(fightId).subscribe(() => {
        console.log('end my suffering');
      });
    });
  }
}
