import { Component, EventEmitter, OnInit, Input } from '@angular/core';
import { BlogService } from '../blog.service';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { Blog, BlogSystemStatus } from '../model/blog.model';
import { BlogStatus } from '../model/blogstatus-model';
import { Rating } from '../model/rating.model';
import { Observable } from 'rxjs';

@Component({
  selector: 'xp-single-blog-display',
  templateUrl: './single-blog-display.component.html',
  styleUrls: ['./single-blog-display.component.css']
})
export class SingleBlogDisplayComponent implements OnInit{

  public selectedBlog : Blog
  public blogId : number
  public rating : Rating = {
    blogId:-1,
    userId:-1,
    username:'a',
    creationTime:new Date().toISOString().split('T')[0],
    rating: 'a'
  }
  public images : string[];
  public hidden: boolean;
  public error_text: string;

  constructor(private service: BlogService, private router: Router, private route: ActivatedRoute) {}

ngOnInit(): void {
  this.route.paramMap.subscribe((params: ParamMap) => {
    this.blogId = Number(params.get('id'));

    if(this.blogId !== 0){
      this.service.getBlog(this.blogId).subscribe({
        next: (res: Blog) => {
          this.hidden = false;
          this.selectedBlog = res;
          this.images = this.selectedBlog.imageLinks[0].split(',');
          this.images.forEach((value, index, array) => {
            if (value.includes('(') || value.includes(')')) {
              array[index] = value.replace(/[()]/g, '');
            }
          });  
        },
        error: (err: any) => {
          this.hidden = true;
          this.error_text = "You are not following this blog creator.";
          console.log(err);
        }
        

      })
    }
  });
}

rate(x:number): void{
  this.rating.blogId = this.blogId;
  this.rating.creationTime = new Date().toISOString().split('T')[0];

  if(x === 1){
    this.rating.rating = 'UPVOTE';
  }
  if(x === 2){
    this.rating.rating = 'DOWNVOTE';
  }
 
  this.service.addRating(this.rating).subscribe({
    next: (result: Blog) => {
      this.selectedBlog.blogRatings = result.blogRatings;
      this.ngOnInit();
    },
    error: (err: any) => {
        console.log(err);
      }
  });
}
}
