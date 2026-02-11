import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { PersonModel } from '../../Models/PersonModel';
import { Observable, tap } from 'rxjs';
import { environment } from '../../environments/environment';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root',
})
export class PersonService {

  private apiUrl = `${environment.apiUrl}/Person`;
  private authUrl = `${environment.apiUrl}/Auth`;

  constructor(private http: HttpClient) { }

  getPeople(): Observable<PersonModel[]> {
    return this.http.get<PersonModel[]>(`${this.apiUrl}`);
  }

  register(person: PersonModel): Observable<any> {
    return this.http.post(this.apiUrl, person);
  }

  login(username: string, password: string): Observable<any> {
    return this.http.post<any>(`${this.authUrl}/login`, { username, password }).pipe(
      tap(res => {
        if (res && res.token) {
          localStorage.setItem('token', res.token);
          console.log('Token saved successfully!');
        } else {
          console.error('No token found in response');
        }
      })
    );
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }

  logout(): void {
    localStorage.removeItem('token');
  }

  getUserRole(): string | null {
    const token = localStorage.getItem('token');
    if (!token) return null;

    try {
      const decodedToken: any = jwtDecode(token);
      return decodedToken.role ||
        decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] ||
        null;
    } catch (e) {
      return null;
    }
  }

  isAdmin(): boolean {
    return this.getUserRole() == 'Admin';
  }

  getUserId(): number | null {
    const token = localStorage.getItem('token');
    if (!token) return null;
    try {
      const decodedToken: any = jwtDecode(token);
      return decodedToken.userId || decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] || null;
    } catch (e) {
      return null;
    }
  }

  getPersonName(): string {
    const token = localStorage.getItem('token');
    if (!token) return 'אורח';
    try {
      const decoded: any = jwtDecode(token);
      return decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] || 'משתמש';
    } catch (e) {
      console.error('שגיאה בפענוח השם מהטוקן', e);
      return 'אורח';
    }
  }
}