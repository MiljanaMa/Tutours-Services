import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home/home.component';
import { NavbarComponent } from './navbar/navbar.component';
import { MaterialModule } from 'src/app/infrastructure/material/material.module';
import { RouterModule } from '@angular/router';
import { ProfileComponent } from '../administration/profile/profile.component';
import { AdministrationModule } from '../administration/administration.module';
import {MatGridListModule} from '@angular/material/grid-list';
import { FooterComponent } from './footer/footer.component';
import {MatSidenavModule} from '@angular/material/sidenav';
import { MarketplaceModule } from "../marketplace/marketplace.module";
import { MatMenuModule } from '@angular/material/menu';
import {FormsModule} from "@angular/forms";

@NgModule({
  declarations: [
    HomeComponent,
    NavbarComponent,
    FooterComponent,
  ],
  imports: [
    CommonModule,
    MaterialModule,
    RouterModule,
    AdministrationModule,
    MatGridListModule,
    MatSidenavModule,
    MarketplaceModule,
    MatMenuModule,
    MarketplaceModule,
    FormsModule,
  ],
  exports: [
    NavbarComponent,
    HomeComponent,
    AdministrationModule,
    MatGridListModule,
    FooterComponent
  ]
})
export class LayoutModule { }
