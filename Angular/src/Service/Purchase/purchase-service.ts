import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PresentModel } from '../../Models/PresentModel';
import { PurchaseModel } from '../../Models/PurchaseModel';

@Injectable({
  providedIn: 'root',
})
export class PurchaseService {

  private apiUrl = `${environment.apiUrl}/Purchase`;

  constructor(private http: HttpClient) { }

  addPurchase(personId: number, basketId: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/addPurchase/${personId}/${basketId}`, {});
  }

  GetAllPurchasesOfPresent(presentId: number): Observable<any> {
    return this.http.get<PurchaseModel[]>(`${this.apiUrl}/purchases/${presentId}`);
  }

  sortPresentsByPrice(): Observable<any> {
    return this.http.get<PresentModel[]>(`${this.apiUrl}/presents/sorted-by-price`);
  }

  GetSortPresentsByPopular(): Observable<any> {
    return this.http.get<PresentModel[]>(`${this.apiUrl}/presents/sorted-by-popular`);
  }

  GetAllBuyersOfPresent(): Observable<any> {
    return this.http.get(`${this.apiUrl}/buyers`);
  }
}