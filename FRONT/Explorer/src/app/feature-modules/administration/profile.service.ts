import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PagedResults } from 'src/app/shared/model/paged-results.model';
import { Profile } from './model/profile.model';
import { ActivatedRoute } from '@angular/router';
import { environment } from 'src/env/environment';
import { ChatMessage } from './model/chat-preview.model';
import { MessageInput } from './model/message-input.model';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {

  constructor(private http: HttpClient) {

  }

  getProfile(userId: number): Observable<Profile> {
    return this.http.get<Profile>(`${environment.apiHost}profile/${userId}`);
  }

  getPerson(userId: number): Observable<Profile> {
    return this.http.get<Profile>(`${environment.apiHost}profile/zelimdaumrem/${userId}`);
  }

  getFollowers(): Observable<PagedResults<Profile>> {
    return this.http.get<PagedResults<Profile>>(`${environment.apiHost}profile/followers`);
  }

  getFollowing(): Observable<PagedResults<Profile>> {
    return this.http.get<PagedResults<Profile>>(`${environment.apiHost}profile/following`);
  }

  unfollow(followingId: number): Observable<any> {
    return this.http.delete(`${environment.apiHost}profile/unfollow/${followingId}`);
  }

  follow(followingId: number): Observable<any> {
    return this.http.post(`${environment.apiHost}profile/follow/${followingId}`, followingId);
  }

  updateProfile(userId: number, updatedProfile: Profile): Observable<Profile> {
    return this.http.put<Profile>(`${environment.apiHost}profile/${userId}`, updatedProfile);
  }

  getRecommendedProfiles(): Observable<PagedResults<Profile>> {
    return this.http.get<PagedResults<Profile>>(environment.apiHost + `profile/get-recommendations`);
  }

  getPreviewChats(): Observable<ChatMessage[]>{
    return this.http.get<ChatMessage[]>(`${environment.apiHost}chat/preview`);
  }

  getMessages(followerId: number): Observable<PagedResults<ChatMessage>>{
    return this.http.get<PagedResults<ChatMessage>>(`${environment.apiHost}chat/${followerId}`);
  }

  sendMessage(message: MessageInput): Observable<ChatMessage>{
    return this.http.post<ChatMessage>(`${environment.apiHost}chat`, message);
  }
}
