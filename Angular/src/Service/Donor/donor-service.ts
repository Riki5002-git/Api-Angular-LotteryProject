import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DonorModel } from '../../Models/DonorModel';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class DonorService {

  private apiUrl = `${environment.apiUrl}/Donor`;

  constructor(private http: HttpClient) { }

  getDonors(): Observable<DonorModel[]> {
    return this.http.get<DonorModel[]>(this.apiUrl);
  }

  addDonor(donor: DonorModel): Observable<DonorModel> {
    return this.http.post<DonorModel>(this.apiUrl, donor);
  }

  deleteDonor(donorId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${donorId}`);
  }

  updateDonor(donor: DonorModel): Observable<DonorModel> {
    return this.http.put<DonorModel>(`${this.apiUrl}/${donor.id}`, donor);
  }

  getDonorsPresents(donorId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/${donorId}/presents`);
  }

   filterDonorsByName(fullName: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/name/${encodeURIComponent(fullName)}`);
  }

  filterDonorsByEmail(email: string): Observable<DonorModel[]> {
    return this.http.get<DonorModel[]>(`${this.apiUrl}/email/${email}`);
  }

  filterDonorsByPresent(presentName: string): Observable<DonorModel[]> {
    return this.http.get<DonorModel[]>(`${this.apiUrl}/present/${presentName}`);
  }
}