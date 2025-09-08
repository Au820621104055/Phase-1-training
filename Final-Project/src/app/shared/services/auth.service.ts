import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LoginRequest, RegisterRequest, AuthResponse } from '../models/auth.interface';
import { jwtDecode } from 'jwt-decode';

interface JwtPayload {
  nameid: string;     
  unique_name: string;  
  role: string;         
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:7279/api/Auth';

  constructor(private http: HttpClient) {}

  login(credentials: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, credentials);
  }

  register(user: RegisterRequest): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/register`, user);
  }

  logout(): void {
    localStorage.clear();
  }

  saveAuthData(response: AuthResponse): void {
  localStorage.setItem('authToken', response.token);

  try {
    const decoded: any = jwtDecode(response.token);
    console.log('Decoded JWT payload:', decoded);

    const role =
      decoded.role ||
      decoded.roles ||
      decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
    const name =
      decoded.name ||
      decoded.unique_name ||
      decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] ||
      '';
    const userId =
      decoded.nameid ||
      decoded.userId ||
      decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];

    if (role) localStorage.setItem('role', role);
    if (name) localStorage.setItem('fullName', name);
    if (userId) localStorage.setItem('userId', userId);

  } catch (e) {
    console.error('Failed to decode JWT:', e);
  }
}


  getRole(): string | null {
    return localStorage.getItem('role');
  }

  getUserId(): number | null {
    const id = localStorage.getItem('userId');
    return id ? parseInt(id, 10) : null;
  }

  getFullName(): string | null {
    return localStorage.getItem('fullName');
  }

  isLoggedIn(): boolean {
  const token = localStorage.getItem('authToken');
  console.log('Auth token in storage:', token);
  return !!token;
}
}
