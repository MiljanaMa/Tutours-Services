import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PagedResults } from 'src/app/shared/model/paged-results.model';
import { environment } from 'src/env/environment';
import { NotificationModel } from './model/notification.model';

@Injectable({
  providedIn: 'root'
})
export class LayoutService {

  constructor(private http: HttpClient) { }

  getNotifications(): Observable<PagedResults<NotificationModel>> {
    return this.http.get<PagedResults<NotificationModel>>(`${environment.apiHost}notification/byUser`);
  }

  markAsRead(notification: NotificationModel): Observable<NotificationModel> {
    return this.http.put<NotificationModel>(`${environment.apiHost}notification/${notification.id}`, notification);
  }

  delete(notificationId: number): Observable<NotificationModel> {
    return this.http.delete<NotificationModel>(`${environment.apiHost}notification/${notificationId}`);
  }
}
