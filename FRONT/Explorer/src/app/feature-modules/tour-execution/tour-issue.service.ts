import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PagedResult } from './shared/model/paged-result.model';
import { TourIssue } from './model/tour-issue.model';
import { environment } from 'src/env/environment';
import { Tour } from '../tour-authoring/model/tour.model';
import { TourIssueComment } from './model/tour-issue-comment.model';

@Injectable({
  providedIn: 'root'
})
export class TourIssueService {

  constructor(private http: HttpClient) { }

  getTourIssues(): Observable<PagedResult<TourIssue>> {
    return this.http.get<PagedResult<TourIssue>>(environment.apiHost + 'tourexecution/tourissue');
  }

  getTourIssue(id : Number): Observable<PagedResult<TourIssue>> {
    return this.http.get<PagedResult<TourIssue>>(`${environment.apiHost}tourexecution/tourissue/id/${id}`);
  }

  getTourIssueByUserId(id : Number): Observable<PagedResult<TourIssue>> {
    return this.http.get<PagedResult<TourIssue>>(`${environment.apiHost}tourexecution/tourissue/user/${id}`);
  }

  addTourIssue(tourissue: TourIssue): Observable<TourIssue> {
    return this.http.post<TourIssue>(environment.apiHost + 'tourexecution/tourissue', tourissue);
  }

  updateTourIssue(tourissue: TourIssue): Observable<TourIssue> {
    return this.http.put<TourIssue>(environment.apiHost + 'tourexecution/tourissue/'+ tourissue.id, tourissue);
  }

  resolveTourIssue(tourissue: TourIssue): Observable<TourIssue> {
    return this.http.put<TourIssue>(environment.apiHost + 'tourexecution/tourissue/resolve/'+ tourissue.id, tourissue);
  }

  deleteTourIssue(tourissueId: number): Observable<TourIssue> {
    return this.http.delete<TourIssue>(environment.apiHost + 'tourexecution/tourissue/'+ tourissueId);
  }

  getTour(tourId: number): Observable<Tour> {
    return this.http.get<Tour>(`${environment.apiHost}author/tours/${tourId}`);
  }

  updateTour(updatedTour: Tour): Observable<Tour>{
    return this.http.put<Tour>(`${environment.apiHost}author/tours/disable/${updatedTour.id}`, updatedTour);
  }

  getTourIssueComments(): Observable<PagedResult<TourIssueComment>> {
    return this.http.get<PagedResult<TourIssueComment>>(environment.apiHost + 'tourexecution/tourissuecomment');
  }

  addTourIssueComment(tourissue: TourIssueComment): Observable<TourIssueComment> {
    return this.http.post<TourIssueComment>(environment.apiHost + 'tourexecution/tourissuecomment', tourissue);
  }

  updateTourIssueComment(tourissue: TourIssueComment): Observable<TourIssueComment> {
    return this.http.put<TourIssueComment>(environment.apiHost + 'tourexecution/tourissuecomment/'+ tourissue.id, tourissue);
  }

  deleteTourIssueComment(tourissueId: number): Observable<TourIssueComment> {
    return this.http.delete<TourIssueComment>(environment.apiHost + 'tourexecution/tourissuecomment/'+ tourissueId);
  }

  setResolveDateTime(tourissue: TourIssue): Observable<TourIssue> {
    return this.http.put<TourIssue>(environment.apiHost + 'tourexecution/tourissue/setresolvedate/' + tourissue.id, tourissue);
  }
}
