import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './infrastructure/routing/app-routing.module';
import { AppComponent } from './app.component';
import { LayoutModule } from './feature-modules/layout/layout.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './infrastructure/material/material.module';
import { AdministrationModule } from './feature-modules/administration/administration.module';
import { BlogModule } from './feature-modules/blog/blog.module';
import { TouristModule } from './feature-modules/tourist/tourist.module';
import { MarketplaceModule } from './feature-modules/marketplace/marketplace.module';
import { TourAuthoringModule } from './feature-modules/tour-authoring/tour-authoring.module';
import { TourExecutionModule } from './feature-modules/tour-execution/tour-execution.module';
import { AuthModule } from './infrastructure/auth/auth.module';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { JwtInterceptor } from './infrastructure/auth/jwt/jwt.interceptor';
import { MarkdownModule, MarkdownService } from 'ngx-markdown';
import { FormsModule } from '@angular/forms';
import { MapComponent } from './shared/map/map.component';
import { TimePipe } from './shared/helpers/time.pipe';
import { EncountersModule } from './feature-modules/encounters-managing/encounters.module';
import { MatNativeDateModule, NativeDateAdapter, MAT_DATE_FORMATS, DateAdapter } from '@angular/material/core';
import { ToastModule} from 'primeng/toast'


@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    LayoutModule,
    BrowserAnimationsModule,
    MaterialModule,
    AdministrationModule,
    BlogModule,
    MarketplaceModule,
    TourAuthoringModule,
    TourExecutionModule,
    TouristModule,
    AuthModule,
    HttpClientModule,
    TouristModule,
    MarkdownModule.forRoot({ loader: HttpClientModule }),
    FormsModule,
    EncountersModule,
    MatNativeDateModule,
    ToastModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptor,
      multi: true,
    },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
