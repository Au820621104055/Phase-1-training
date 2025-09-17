import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { User } from '../models/user.model';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private apiBase = '/api/Auth';

  constructor(private http: HttpClient) {}

  login(email: string, password: string): Observable<{ token: string; user: User }> {
    return this.http.post<{ token: string; user: User }>(`${this.apiBase}/login`, { email, password })
      .pipe(tap(res => {
        if (res?.token) {
          localStorage.setItem('token', res.token);
          localStorage.setItem('user', JSON.stringify(res.user));
        }
      }));
  }

  register(payload: any): Observable<any> {
    return this.http.post(`${this.apiBase}/register`, payload);
  }

  logout() { localStorage.removeItem('token'); localStorage.removeItem('user'); }
  getToken(): string | null { return localStorage.getItem('token'); }
  getUser(): User | null { const u = localStorage.getItem('user'); return u ? JSON.parse(u) : null; }
  isLoggedIn(): boolean { return !!this.getToken(); }
}
