import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {MatToolbar, MatToolbarModule,} from '@angular/material/toolbar';
import {MatButton, MatButtonModule, MatIconButton} from '@angular/material/button';
import {MatFormField, MatFormFieldModule, MatLabel} from '@angular/material/form-field';
import {MatInput, MatInputModule} from '@angular/material/input';
import {MatTable, MatTableModule} from '@angular/material/table';
import {MatIcon, MatIconModule} from '@angular/material/icon';
import {MatList, MatListModule} from '@angular/material/list';
import { MatSelectModule } from '@angular/material/select';
import { MatOptionModule } from '@angular/material/core';
import {MatDialogModule} from '@angular/material/dialog';
import {MatButtonToggleModule} from '@angular/material/button-toggle';
import {MatCard, MatCardModule} from '@angular/material/card';
import { MatCheckbox, MatCheckboxModule } from '@angular/material/checkbox';

@NgModule({
  declarations: [],
  imports: [
    MatToolbarModule,
    CommonModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatTableModule,
    MatIconModule,
    MatListModule,
    MatSelectModule,
    MatOptionModule,
    MatDialogModule,
    MatButtonToggleModule,
    MatCardModule,
    MatCheckboxModule,
  ],
  exports: [
    MatToolbar,
    MatButton,
    MatFormField,
    MatLabel,
    MatInput,
    MatTable,
    MatIconButton,
    MatIcon,
    MatListModule,
    MatSelectModule,
    MatOptionModule,
    MatDialogModule,
    MatButtonToggleModule,
    MatCard,
    MatCheckbox,
  ]
})
export class MaterialModule { }
