import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Keypoint } from '../model/keypoint.model';

@Component({
  selector: 'xp-public-keypoint',
  templateUrl: './public-keypoint.component.html',
  styleUrls: ['./public-keypoint.component.css']
})
export class PublicKeypointComponent {
  @Output() keypointSelected = new EventEmitter<Keypoint>();
  @Input() publicKeypoints : Keypoint[];

  constructor() { }
  
  onSelectClicked(keypoint: Keypoint): void{
    this.keypointSelected.emit(keypoint);
  }
}
