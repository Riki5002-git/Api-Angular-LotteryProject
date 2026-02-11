import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable, Subject, tap } from 'rxjs';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root',
})
export class BasketService {

  private apiUrl = `${environment.apiUrl}/Basket`;
  private basketUpdated = new Subject<void>();
  basketUpdated$ = this.basketUpdated.asObservable();

  constructor(private http: HttpClient) { }

  addToBasket(personId: number, presentId: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/addToBasket/${personId}/${presentId}`, {}).pipe(
      tap(() => this.basketUpdated.next())
    );
  }

  removeFromBasket(personId: number, presentId: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/removeFromBasket/${personId}/${presentId}`, {});
  }

  getMyBasket(personId: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/getMyBasket/${personId}`).pipe(
      tap((res: any) => {
        const id = res?.id || res?.basketId;
        if (id) {
          localStorage.setItem('basketId', id.toString());
          console.log('Basket ID saved:', id);
        } else {
          console.error('מבנה הנתונים מהשרת לא מכיל ID:', res);
        }
      })
    );
  }

  clearItemCompletely(personId: number, presentId: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/clearItemCompletely/${personId}/${presentId}`);
  }

  getBasketId(): string | null {
    return localStorage.getItem('basketId');
  }
}