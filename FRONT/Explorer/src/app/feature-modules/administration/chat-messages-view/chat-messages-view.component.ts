import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { ProfileService } from '../profile.service';
import { ChatMessage } from '../model/chat-preview.model';
import { MessageInput } from '../model/message-input.model';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';

@Component({
  selector: 'xp-chat-messages-view',
  templateUrl: './chat-messages-view.component.html',
  styleUrls: ['./chat-messages-view.component.css']
})
export class ChatMessagesViewComponent implements OnChanges {

  @Input() followerId: number;
  @Output() messageSent = new EventEmitter<void>();

  public messages: ChatMessage[] = [];
  public messageContent: string;
  public userId: number;

  constructor(private profileService: ProfileService, private authService: AuthService){
    this.userId = authService.user$.value.id;
    console.log("Korisnik id: " , this.userId);
  }

  ngOnChanges(): void {
    if(this.followerId > 0){
      this.getMessages();
    }
  }

  getMessages(): void{
    this.profileService.getMessages(this.followerId).subscribe(res => {
      this.messages = res.results;
      this.messageSent.emit();
    });    
  }

  sendMessage(){
    if(this.messageContent){
      let message: MessageInput = {
        receiverId: this.followerId,
        content: this.messageContent
      }
      this.profileService.sendMessage(message).subscribe(res => {
        this.messageContent = '';
        this.getMessages();
      });
    }
  }
}
