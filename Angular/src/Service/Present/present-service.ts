import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { PresentModel } from '../../Models/PresentModel';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { CategoryModel } from '../../Models/CategoryModel';

@Injectable({
  providedIn: 'root',
})
export class PresentService {

  private apiUrl = `${environment.apiUrl}/Present`;

  constructor(private http: HttpClient) { }

  getPresents(): Observable<PresentModel[]> {
    return this.http.get<PresentModel[]>(this.apiUrl);
  }

  addPresent(present: PresentModel): Observable<PresentModel> {
    return this.http.post<PresentModel>(this.apiUrl, present);
  }

  getCategories(): Observable<CategoryModel[]> {
    return this.http.get<CategoryModel[]>(`${environment.apiUrl}/Category`);
  }

  deletePresent(presentId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${presentId}`);
  }

  updatePresent(present: PresentModel): Observable<PresentModel> {
    return this.http.put<PresentModel>(`${this.apiUrl}/${present.id}`, present);
  }
}