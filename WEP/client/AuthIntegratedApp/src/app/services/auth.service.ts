import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = 'https://localhost:5025/api'; // Replace with your actual API URL
  private token: string | null = null;

  constructor(private http: HttpClient) { }

  login(token: { accessToken: string }): Observable<any> {
    return this.http.post(`${this.apiUrl}/authenticate`, token);
  }

  getDashboardData(): Observable<any> {
    return this.http.get(`${this.apiUrl}/home`);
  }

  getToken(): string | null {
    return this.token;
  }

  setToken(token: string) {
    this.token = token;
  }
}