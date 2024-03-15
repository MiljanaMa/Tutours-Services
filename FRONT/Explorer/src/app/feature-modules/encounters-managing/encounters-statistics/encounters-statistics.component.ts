import { Component, OnChanges, OnInit } from '@angular/core';
import Highcharts from 'highcharts/es-modules/masters/highcharts.src';
import { EncountersService } from '../encounters.service';
import { TourYearStats } from '../model/tour-year-stats.model';
import { EncounterYearStats } from '../model/encounter-year-stats.model';
import { EncounterStats } from '../model/encounter-stats.model';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'xp-encounters-statistics',
  templateUrl: './encounters-statistics.component.html',
  styleUrls: ['./encounters-statistics.component.css']
})
export class EncountersStatisticsComponent implements OnInit{

  public highcharts = Highcharts;
  public encounterChartOptions:  Highcharts.Options | null = null;
  public encounterChartOptionsByMonths:  Highcharts.Options | null = null;
  public tourChartOptionsByMonths:  Highcharts.Options | null = null;
  public currentDate: Date = new Date();
  public encounterStatsYear: number = this.currentDate.getFullYear();
  public tourStatsYear: number = this.currentDate.getFullYear();

  constructor(private service: EncountersService, public dialogRef: MatDialogRef<EncountersStatisticsComponent>) { }

  ngOnInit(): void {
    this.getEncounterStatistics();
    this.getEncounterYearStatistics();
    this.getTourYearStatistics();
  }

  getEncounterStatistics(): void {
    this.service.getEncounterStats().subscribe({
      next: (result: EncounterStats) => {;
        this.encounterChartOptions = {
          chart: {
            type: 'pie',
          },
          title: {
            text: 'Completed vs Failed Encounters in total',
          },
          series: [
            {
              type: 'pie',
              name: 'Encounter Status',
              data: [
                {
                  name: 'Completed',
                  y: result.completedCount
                },
                {
                  name: 'Failed',
                  y: result.failedCount
                },
              ],
            },
          ],
        };
      },
      error: (error) => {
        
      },
    });  
  }

  getEncounterYearStatistics(): void {
    this.service.getEncounterYearStats(this.encounterStatsYear).subscribe({
      next: (result: EncounterYearStats) => {
        this.encounterChartOptionsByMonths = {
          chart: {
            type: 'column',
          },
          title: {
            text: 'Encounter Completions in ' + this.encounterStatsYear,
          },
          xAxis: {
            categories: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"]
          },
          series: [
            {
              name: 'Completed',
              data: result.completedCountByMonths,
              type: 'column' 
            },
            {
              name: 'Failed',
              data: result.failedCountByMonths,
              type: 'column' 
            }
          ],
        };
      },
      error: (error) => {
        
      },
    });  

  }

  getTourYearStatistics(): void {
    this.service.getTourYearStats(this.tourStatsYear).subscribe({
      next: (result: TourYearStats) => {
        this.tourChartOptionsByMonths = {
          chart: {
            type: 'spline',
          },
          title: {
            text: 'Tour Completions in ' + this.tourStatsYear,
          },
          xAxis: {
            categories: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"]
          },
          series: [
            {
              name: 'Completed',
              data: result.completedCountByMonths,
              type: 'spline' 
            }
          ],
        };
        // Update the chart by re-rendering it with new options
        if (this.highcharts.charts[0]) {
        this.highcharts.charts[0].update(this.tourChartOptionsByMonths, true);
        }
      },
      error: (error) => {
        
      },
    });  
   }

   closeModal(): void {
    this.dialogRef.close();
  }

  increaseTourStatsYear(): void {
    if(this.tourStatsYear < this.currentDate.getFullYear()){
      this.tourStatsYear += 1;
      this.getTourYearStatistics();
    }
  }

  reduceTourStatsYear(): void {
    if(this.tourStatsYear > 1970){
      this.tourStatsYear -= 1;
      this.getTourYearStatistics();
    }
  }

  increaseEncounterStatsYear(): void {
    if(this.encounterStatsYear < this.currentDate.getFullYear()){
      this.encounterStatsYear += 1;
      this.getEncounterYearStatistics();
    }
  }

  reduceEncounterStatsYear(): void {
    if(this.encounterStatsYear > 1970){
      this.encounterStatsYear -= 1;
      this.getEncounterYearStatistics();
    }
  }

}
