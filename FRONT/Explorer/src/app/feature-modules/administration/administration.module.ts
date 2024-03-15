import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EquipmentFormComponent } from './equipment-form/equipment-form.component';
import { EquipmentComponent } from './equipment/equipment.component';
import { MaterialModule } from 'src/app/infrastructure/material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppRatingComponent } from './app-rating/app-rating.component';
import { ProfileComponent } from './profile/profile.component';
import { AppRatingFormAuthorComponent } from './app-rating-form-author/app-rating-form-author.component';
import { AppRatingTouristComponent } from './app-rating-tourist/app-rating-tourist.component';
import { AppRatingAuthorComponent } from './app-rating-author/app-rating-author.component';
import { AppRatingFormTouristComponent } from './app-rating-form-tourist/app-rating-form-tourist.component';
import { ChatViewComponent } from './chat-view/chat-view.component';
import { ChatCardComponent } from './chat-card/chat-card.component';
import { ChatMessagesViewComponent } from './chat-messages-view/chat-messages-view.component';
import { MatSelectModule } from '@angular/material/select';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
import { PublicEntityRequestsComponent } from './public-entity-requests/public-entity-requests.component';




@NgModule({
  declarations: [
    EquipmentFormComponent,
    EquipmentComponent,
    AppRatingComponent,
    ProfileComponent,
    AppRatingFormAuthorComponent,
    AppRatingTouristComponent,
    AppRatingAuthorComponent,
    AppRatingFormTouristComponent,
    ChatViewComponent,
    ChatCardComponent,
    ChatMessagesViewComponent,
    PublicEntityRequestsComponent
  ],
  imports: [
    CommonModule,
    MaterialModule,
    ReactiveFormsModule,
    FormsModule,
    MatSelectModule,
    NgxMatSelectSearchModule
  ],
  exports: [
    EquipmentComponent,
    EquipmentFormComponent,
    AppRatingComponent,
    ProfileComponent,
    AppRatingFormAuthorComponent,
    AppRatingTouristComponent,
    AppRatingAuthorComponent,
    AppRatingFormTouristComponent,
    PublicEntityRequestsComponent
  ]
})
export class AdministrationModule { }
