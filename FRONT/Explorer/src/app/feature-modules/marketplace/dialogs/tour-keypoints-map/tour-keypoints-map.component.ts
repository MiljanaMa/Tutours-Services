import { Component, EventEmitter, Inject, Input, OnInit, Output } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Keypoint } from 'src/app/feature-modules/tour-authoring/model/keypoint.model';
import { Tour } from 'src/app/feature-modules/tour-authoring/model/tour.model';
import { RouteInfo } from 'src/app/shared/model/routeInfo.model';
import { RouteQuery } from 'src/app/shared/model/routeQuery.model';

@Component({
  selector: 'xp-tour-keypoints-map',
  templateUrl: './tour-keypoints-map.component.html',
  styleUrls: ['./tour-keypoints-map.component.css']
})
export class TourKeypointsMapComponent implements OnInit {
  @Output() keypointsUpdated = new EventEmitter<Keypoint>();
  @Output() routeFound = new EventEmitter<RouteInfo>();
  @Input() routeQuery: RouteQuery;
  public tour: Tour;;

  constructor(public dialogRef: MatDialogRef<TourKeypointsMapComponent>, @Inject(MAT_DIALOG_DATA) public selectedTour: Tour) {
    this.routeQuery = {
      keypoints: selectedTour.keypoints ?? [],
      transportType: selectedTour.transportType
    };
    this.tour = selectedTour;
  }
  ngOnInit(): void {

  }
  onNoClick(): void {
    this.dialogRef.close();
  }

  routeFoundEmit(routeInfo: RouteInfo) {
    this.routeFound.emit(routeInfo);
  }

}
