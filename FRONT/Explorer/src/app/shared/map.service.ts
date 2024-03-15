import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MapService {

  /*
  * API Docs:
  * https://nominatim.org/release-docs/latest/api/Overview/
  * https://github.com/Jorl17/open-elevation/blob/master/docs/api.md
  * 
  */
  private readonly geoCodingApi = 'https://nominatim.openstreetmap.org/search?format=json';
  private readonly reverseGeoCodingApi = 'https://nominatim.openstreetmap.org/reverse?format=json';
  private readonly elevationApi = 'https://api.open-elevation.com/api/v1/lookup?';

  constructor(private http: HttpClient) {}

  search(street: string): Observable<any> {
    return this.http.get(`${this.geoCodingApi}&q=${street}`);
  }

  reverseSearch(lat: number, lon: number): Observable<any> {
    return this.http.get(`${this.reverseGeoCodingApi}&lat=${lat}&lon=${lon}`);
  }

  getElevation(): Observable<any> {
    return this.http.get(`${this.elevationApi}locations=10,10|20,20|41.161758,-8.583933`);
  }
}
