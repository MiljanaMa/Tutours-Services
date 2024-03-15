import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ChatMessage } from '../model/chat-preview.model';

@Component({
  selector: 'xp-chat-card',
  templateUrl: './chat-card.component.html',
  styleUrls: ['./chat-card.component.css']
})
export class ChatCardComponent implements OnInit{

  @Input() chatPreviewMessage: ChatMessage;
  @Output() chatSelected = new EventEmitter<Number>;
  public userId = -1;

  constructor(){}

  ngOnInit(): void{
    this.userId = parseInt(localStorage.getItem('loggedId')??'1');
  }

  selectChat(): void{
    if(this.userId !== this.chatPreviewMessage.sender.id){
      this.chatSelected.emit(this.chatPreviewMessage.sender.id);
    }else{
      this.chatSelected.emit(this.chatPreviewMessage.receiver.id);
    }
  }
}
