import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class LotteryService {

  private apiUrl = `${environment.apiUrl}/Lottery`;

  constructor(private http: HttpClient) { }

  Lottery(presentId: number): Observable<any> {
    return this.http.post(`${this.apiUrl}?presentId=${presentId}`, {});
  }
}
