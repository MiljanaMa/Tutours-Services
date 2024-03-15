import { Component, OnInit } from '@angular/core';
import { ProfileService } from '../profile.service';
import { ChatMessage } from '../model/chat-preview.model';
import { Profile } from '../model/profile.model';
import { FormControl } from '@angular/forms';
import { ReplaySubject, Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'xp-chat-view',
  templateUrl: './chat-view.component.html',
  styleUrls: ['./chat-view.component.css']
})
export class ChatViewComponent implements OnInit {


  //https://www.npmjs.com/package/ngx-mat-select-search#try-it
  public followerCtrl: FormControl<Profile | null> = new FormControl<Profile | null>(null);
  public followerFilterCtrl: FormControl<string | null> = new FormControl<string>('');
  public filteredFollowers: ReplaySubject<Profile[]> = new ReplaySubject<Profile[]>(1);
  protected _onDestroy = new Subject<void>();

  public chatPreviews: ChatMessage[] = [];
  public selectedChatId: number = -1;
  public followers: Profile[] = [];

  constructor(private profileService: ProfileService){}

  ngOnInit(): void {
    this.getChatPreviews();

    this.profileService.getFollowers().subscribe(res => {
      this.followers = res.results;
    });

    this.followerFilterCtrl.valueChanges
    .pipe(takeUntil(this._onDestroy))
    .subscribe(() => {
      this.filterFollowers();
    });
  }

  getChatPreviews(): void{
    this.profileService.getPreviewChats().subscribe(res => {
      this.chatPreviews = res;
      this.createAdvertisementMessage();
    });
  }

  chatSelected(event: Number): void{
    this.selectedChatId = event.valueOf();
  }

  private createAdvertisementMessage(): void{
    let adMessage: ChatMessage = {
      content: 'Become pro forklift  player with Tutor',
      creationDateTime: new Date(),
      receiver: {
        name: '',
        surname: '',
        userId: parseInt(localStorage.getItem('loggedId')??'1'),
      },
      sender: {
        name: 'AD: Tutor',
        surname: 'Gaming',
        userId: -1,
        profileImage: 'https://avatars.githubusercontent.com/u/65507197?s=200&v=4'
      }
    }
    this.chatPreviews.push(adMessage);
  }

  private filterFollowers() {
    if (!this.followers) {
      return;
    }

    let search = this.followerFilterCtrl.value;
    if (!search) {
      this.filteredFollowers.next(this.followers.slice());
      return;
    } else {
      search = search.toLowerCase();
    }
    
    this.filteredFollowers.next(
      this.followers.filter(follower => follower.name.toLowerCase().indexOf(search || '') > -1 || follower.surname.toLowerCase().indexOf(search || '') > -1)
    );
  }

  public createNewChat(followerId: number){
    this.selectedChatId = followerId;
  }
}
