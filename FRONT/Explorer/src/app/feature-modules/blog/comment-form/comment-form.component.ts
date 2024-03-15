import { Component, EventEmitter,Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { FormControl, FormGroup,Validators } from '@angular/forms';
import { CommentService } from '../comment.service';
import { Comment } from '../model/comment.model';
import { Blog } from '../model/blog.model';

@Component({
  selector: 'xp-comment-form',
  templateUrl: './comment-form.component.html',
  styleUrls: ['./comment-form.component.css']
})

export class CommentFormComponent implements OnChanges {

@Input() comment : Comment;
@Input() editMode: boolean = false;
@Input() blog: Blog
@Output() commentAdded = new EventEmitter<null>();
public userId : number

constructor(private service: CommentService) {}
 
ngOnChanges(changes: SimpleChanges): void {
  this.commentForm.reset();
  if(this.editMode == true){
    this.commentForm.patchValue(this.comment);
  }
  }

  commentForm = new FormGroup({
    comment: new FormControl('',[Validators.required]),
  })

  ngOnInit(){
    this.userId = parseInt(localStorage.getItem('loggedId')??'1');
  }

  onSubmit():void{
    if(this.commentForm.valid){
      alert('top');
    }else{
      alert('bottom');
    }
  }

  createComment(): void
  {
    if(this.blog.systemStatus === 'CLOSED'){
      return;
    }

    const newComment: Comment = {
      blogId: this.blog.id,
      postTime: new Date(),
      lastEditTime: new Date(),
      comment: this.commentForm.value.comment || "", 
      isDeleted: false,
      userId: this.userId
    }
    this.service.createComment(newComment).subscribe({
      next: (_) => {
        this.commentAdded.emit();
        this.commentForm.reset();
      }
    });
  }

  updateComment(): void
  {
    if(this.userId !== this.comment.userId || this.blog.systemStatus === 'CLOSED'){
      return
    }

    const com : Comment = {
      id: this.comment.id,
      username: this.comment.username,
      comment: this.commentForm.value.comment || "",
      blogId: this.blog.id,
      userId: this.comment.userId,
      postTime: this.comment.postTime,
      isDeleted: false,
      lastEditTime: new Date()
    }

    this.service.updateComment(com).subscribe({
      next: (_) => {
        this.commentAdded.emit();
        this.commentForm.reset();
      }
    });
  }
}
