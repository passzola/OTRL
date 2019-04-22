import { LandingComponent } from './landing.component';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
// import { SharedModule } from '@custor/modules/shared.module';
import { ActionsComponent } from './actions/actions.component';
import { CommonModule } from '@angular/common';
import { OurServicesComponent } from './our-services/our-services.component';
import {CustomerService} from './our-services/customer-services.service';
// import { ContentHeaderComponent } from '@custor/components/content-header/content-header.component';
import {ContentHeaderModule} from '@custor/components/content-header/content-header.module';
import { FlexLayoutModule } from '@angular/flex-layout';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {MatButtonModule, MatCardModule, MatIconModule, MatToolbarModule,MatSidenavModule} from '@angular/material';

export const routes = [
    { path: '', component: LandingComponent, pathMatch: 'full' }
  ];
@NgModule({
    declarations: [
      LandingComponent,
      ActionsComponent,
      OurServicesComponent,
      // ContentHeaderComponent,

    ],
    imports: [
        RouterModule.forChild(routes),
        ContentHeaderModule,
        CommonModule,
        MatButtonModule, MatCardModule, MatIconModule,
        MatToolbarModule, MatSidenavModule,
        FormsModule, ReactiveFormsModule, FlexLayoutModule
      ],
    providers: [
      CustomerService
      ]
  })
  export class LandingModule { }
