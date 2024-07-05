import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = 'https://localhost:44376/api'; // Replace with your actual API URL

  constructor(private http: HttpClient) { }

  login(login: { username: string, password: string }): Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, login);
  }
}