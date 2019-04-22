import { NgModule } from '@angular/core';
import { MatButtonModule, MatDialogModule,  MatToolbarModule } from '@angular/material';

import { AngConfirmDialogComponent } from './confirm-dialog.component';

@NgModule({
  declarations: [
    AngConfirmDialogComponent
  ],
  imports: [
    MatDialogModule,
    MatButtonModule,
    // MatDividerModule,
    MatToolbarModule
  ],
  entryComponents: [
    AngConfirmDialogComponent
  ],
})
export class AngConfirmDialogModule {
}