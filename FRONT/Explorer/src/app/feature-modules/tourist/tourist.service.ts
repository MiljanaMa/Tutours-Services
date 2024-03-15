import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { EquipmentForSelection } from './model/eqipment-for-selection.model';
import { TouristEquipment } from './model/tourist-equipment.model';
import { Observable } from 'rxjs';
import { PagedResults } from '../../shared/model/paged-results.model';
import { ClubInvitation } from './model/club-invitation.model';
import { environment } from '../../../env/environment';
import { Club } from './model/club.model';
import { ClubJoinRequest } from './model/club-join-request.model';
import { ClubFight } from './model/club-fight.model';
import { ClubFightXPInfo } from './model/club-fight-xp-info.model';
import { ClubChallengeRequest } from './model/club-challenge-request';
import { NewsletterPreference } from './model/newsletter-preference.model';

@Injectable({
  providedIn: 'root'
})
export class TouristService {

  private readonly apiUrl = `${environment.apiHost}tourist`;

  constructor(private http: HttpClient) { }

  

  getEquipmentForSelection(): Observable<EquipmentForSelection[]> {
    return this.http.get<EquipmentForSelection[]>(environment.apiHost + 'tourist/touristEquipment/forSelected/' + parseInt(localStorage.getItem('loggedId')!))
  }

  createSelectionEquipment(touristEquipment : TouristEquipment): Observable<TouristEquipment> {
    return this.http.post<TouristEquipment>(environment.apiHost + 'tourist/touristEquipment', touristEquipment) 
  }
  
  deleteSelectionEquipment(touristEquipment : TouristEquipment): Observable<TouristEquipment> {
    return this.http.post<TouristEquipment>(environment.apiHost + 'tourist/touristEquipment/deleteByTouristAndEquipmentId', touristEquipment) 
  }
  
  getAllClubs():Observable<PagedResults<Club>>{
    return this.http.get<PagedResults<Club>>(environment.apiHost+'tourist/clubs');
  }

  getClubs(): Observable<PagedResults<Club>> {
    return this.http.get<PagedResults<Club>>(environment.apiHost + 'tourist/clubs/byUser');
  }

  addClub(club: Club): Observable<Club>{
    return this.http.post<Club>(environment.apiHost+'tourist/clubs',club)
  }

  updateClub(club: Club): Observable<Club> {
    return this.http.put<Club>(environment.apiHost + 'tourist/clubs/' + club.id, club);
  }

  getClubInvitations(): Observable<PagedResults<ClubInvitation>> {
    return this.http.get<PagedResults<ClubInvitation>>(environment.apiHost + 'tourist/clubInvitation');
  }

  addClubInvitation(clubInvitation: ClubInvitation): Observable<ClubInvitation> {
    return this.http.post<ClubInvitation>(environment.apiHost + 'tourist/clubInvitation', clubInvitation)
  }

  getTouristRequests(): Observable<PagedResults<ClubJoinRequest>>{
    return this.http.get<PagedResults<ClubJoinRequest>>(`${this.apiUrl}/clubJoinRequest`);
  }
  getClubRequests(clubId: number): Observable<PagedResults<ClubJoinRequest>>{
    return this.http.get<PagedResults<ClubJoinRequest>>(`${this.apiUrl}/clubJoinRequest/${clubId}`);
  }
  updateRequest(request: ClubJoinRequest): Observable<ClubJoinRequest> {
    return this.http.put<ClubJoinRequest>(`${this.apiUrl}/clubJoinRequest`, request);
  }
  joinClub(club: Club): Observable<Club>{
    return this.http.post<Club>(`${this.apiUrl}/clubJoinRequest`, club);
  }

  deleteClub(club: Club): Observable<Club>{
    return this.http.delete<Club>(environment.apiHost+'tourist/clubs/'+club.id); 
  }
  getClubsUpdatedModel(): Observable<PagedResults<Club>> {
    return this.http.get<PagedResults<Club>>(environment.apiHost + 'tourist/clubs');
  }

  getClubById(id: number): Observable<Club>{
    return this.http.get<Club>(`${this.apiUrl}/clubs/${id}`);
  }

  getFightById(id: number): Observable<ClubFight> {
    return this.http.get<ClubFight>(`${this.apiUrl}/fight/${id}`);
  }

  getFightXPInfo(fightId: number): Observable<ClubFightXPInfo> {
    return this.http.get<ClubFightXPInfo>(`${environment.apiHost}xp/fight/${fightId}`);
  }
  
  getClubChallenges(id: number): Observable<ClubChallengeRequest[]>{
    return this.http.get<ClubChallengeRequest[]>(`${environment.apiHost}club-challenge-request/club/${id}`);
  }

  acceptChallenge(request: ClubChallengeRequest): Observable<ClubChallengeRequest>{
    return this.http.put<ClubChallengeRequest>(`${environment.apiHost}club-challenge-request/accept`, request);
  }

  declineChallenge(request: ClubChallengeRequest): Observable<ClubChallengeRequest>{
    return this.http.put<ClubChallengeRequest>(`${environment.apiHost}club-challenge-request/decline`, request);
  }

  getFightsByClub(clubId: number): Observable<ClubFight[]> {
    return this.http.get<ClubFight[]>(`${this.apiUrl}/fight/all/${clubId}`);
  }
  
  createChallenge(request: ClubChallengeRequest): Observable<ClubChallengeRequest>{
    return this.http.post<ClubChallengeRequest>(`${environment.apiHost}club-challenge-request`, request);
  }

  updateFights(): any {
    return this.http.get(`${environment.apiHost}xp/fight/update`);
  }
  
  getNewsletterPreference(userid: number): Observable<NewsletterPreference>{
    return this.http.get<NewsletterPreference>(`${environment.apiHost}tourist/newsletterpreference/${userid}`); 
  }

  updateNewsletterPrefence(np: NewsletterPreference): Observable<NewsletterPreference>{
    return this.http.post<NewsletterPreference>(`${environment.apiHost}tourist/newsletterpreference`, np); 
  }

  endFight(fightId: number): any {
    return this.http.get(`${environment.apiHost}tourist/fight/end/${fightId}`);
  }
}
