import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { KeypointComponent } from './keypoint/keypoint.component';
import { MaterialModule } from 'src/app/infrastructure/material/material.module';
import { MatTable, MatTableModule } from '@angular/material/table';
import { KeypointFormComponent } from './keypoint-form/keypoint-form.component';
import { ReactiveFormsModule } from '@angular/forms';
import { ObjectComponent } from './object/object.component';
import { ObjectFormComponent } from './object-form/object-form.component';
import { ToursTestModuleComponent } from './tours-test-module/tours-test-module.component';
import { MapComponent } from 'src/app/shared/map/map.component';
import { ToursEquipmentComponent } from './tours-equipment/tours-equipment.component';
import { ToursPreviewComponent } from './tours-preview/tours-preview.component';
import { TourComponent } from './tour/tour.component';
import { TourFormComponent } from './tour-form/tour-form.component';
import { RouterModule } from '@angular/router';
import { TimePipe } from 'src/app/shared/helpers/time.pipe';
import { PublicKeypointComponent } from './public-keypoint/public-keypoint.component';
import { KeypointEncountersComponent } from './keypoint-encounters/keypoint-encounters.component';
import { KeypointEncountersManagingComponent } from './keypoint-encounters-managing/keypoint-encounters-managing.component';
import { KeypointEncounterFormComponent } from './keypoint-encounter-form/keypoint-encounter-form.component';

@NgModule({
  declarations: [
    KeypointComponent,
    KeypointFormComponent,
    ObjectComponent,
    ObjectFormComponent,
    ToursTestModuleComponent,
    ToursEquipmentComponent,
    ToursPreviewComponent,
    TourComponent,
    TourFormComponent,
    PublicKeypointComponent,
    KeypointEncountersComponent,
    KeypointEncountersManagingComponent,
    KeypointEncounterFormComponent
  ],
  imports: [
    CommonModule,
    MaterialModule,
    ReactiveFormsModule,
    MapComponent,
    RouterModule,
    TimePipe
  ],
  exports: [
    KeypointComponent,
    KeypointFormComponent,
    PublicKeypointComponent
  ]
})
export class TourAuthoringModule { }
