import { Component,Input,Output,OnInit,EventEmitter } from '@angular/core';
import { CommentService } from '../comment.service';
import { Comment } from './../model/comment.model';
import { PagedResults } from 'src/app/shared/model/paged-results.model';
import { Blog } from '../model/blog.model';

@Component({
  selector: 'xp-comments-display',
  templateUrl: './comments-display.component.html',
  styleUrls: ['./comments-display.component.css']
})
export class CommentsDisplayComponent implements OnInit {
  
  @Input() blog : Blog
  @Input() systemStatus : string
  
public editMode : boolean
public comment : Comment
public userId : number

  public comments: Comment[] = []

  constructor(private commentService: CommentService){}

  ngOnInit(): void{
    this.getComments();
    this.userId = parseInt(localStorage.getItem('loggedId')??'1');
  }

  getComments(): void{
    if(this.blog.id === undefined){
      return
    }
    this.commentService.getComments(0,0,this.blog.id).subscribe({
      next : (response: PagedResults<Comment>)=>{
        this.comments = response.results;
        this.comments.reverse();
        this.editMode = false;
      },
      error:(err : any)=>{
        console.log(err);
      }
    });
  }

onUpdateClicked(comment: Comment): void
{
  if(this.userId !== comment.userId || this.blog.systemStatus === 'CLOSED'){
    return
  }
  this.editMode = true;
  this.comment = comment;
}

onDeleteClicked(comment: Comment): void
{
  if(this.userId !== comment.userId || this.blog.systemStatus === 'CLOSED'){
    return
  }
  this.commentService.deleteComment(comment).subscribe({
    next: (_) => {
      this.getComments();
    }
  });
}
}