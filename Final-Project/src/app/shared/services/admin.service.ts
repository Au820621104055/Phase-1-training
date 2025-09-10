import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../models/user.interface';
import { Restaurant } from '../models/restaurant.interface';
import { Order } from '../models/order.interface';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private apiUrl = 'https://localhost:7279/api/Admin';

  constructor(private http: HttpClient) {}

  private getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem('authToken');
    return new HttpHeaders({ 'Authorization': `Bearer ${token}` });
  }

  getAllUsers(): Observable<User[]> {
  return this.http.get<User[]>(`${this.apiUrl}/users`, { headers: this.getAuthHeaders() });
  }

  addUser(user: User): Observable<User> {
    return this.http.post<User>(`${this.apiUrl}/users`, user, { headers: this.getAuthHeaders() });
  }

  updateUser(userId: number, user: User): Observable<User> {
    return this.http.put<User>(`${this.apiUrl}/users/${userId}`, user, { headers: this.getAuthHeaders() });
  }

  deleteUser(userId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/users/${userId}`, { headers: this.getAuthHeaders() });
  }


  updateUserStatus(userId: number, isActive: boolean): Observable<User> {
    return this.http.put<User>(
      `${this.apiUrl}/users/${userId}/status?isActive=${isActive}`,
      {}, 
      { headers: this.getAuthHeaders() }
    );
  }

updateRestaurantStatus(restaurantId: number, newStatus: string): Observable<void> {
  return this.http.put<void>(
    `${this.apiUrl}/restaurants/${restaurantId}/status?status=${newStatus}`,
    {},
    { headers: this.getAuthHeaders() }
  );
}
}
