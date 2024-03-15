import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BlogFormComponent } from './blog-form/blog-form.component';
import { ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from 'src/app/infrastructure/material/material.module';
import { MarkdownModule } from 'ngx-markdown';
import { CommentFormComponent } from './comment-form/comment-form.component';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { CommentsDisplayComponent } from './comments-display/comments-display.component';
import { MatTable, MatTableModule } from '@angular/material/table';
import { SingleBlogDisplayComponent } from './single-blog-display/single-blog-display.component';
import { BlogListDisplayComponent } from './blog-list-display/blog-list-display.component';
import {MatExpansionModule} from '@angular/material/expansion';
import {MatStepperModule} from '@angular/material/stepper';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatMenuModule } from '@angular/material/menu';

@NgModule({
  declarations: [
    BlogFormComponent,
    CommentFormComponent,
    CommentsDisplayComponent,
    SingleBlogDisplayComponent,
    BlogListDisplayComponent,
  ],
  imports: [
    CommonModule,
    MaterialModule,
    ReactiveFormsModule,
    MarkdownModule,
    MatInputModule,
    MatFormFieldModule,
    MatExpansionModule,
    MatStepperModule,
    MatTableModule,
    MatCheckboxModule,
    MatMenuModule
  ],
  exports: [
    CommentFormComponent,
    CommentsDisplayComponent,
    BlogListDisplayComponent,
    BlogFormComponent
  ]
})
export class BlogModule { }
