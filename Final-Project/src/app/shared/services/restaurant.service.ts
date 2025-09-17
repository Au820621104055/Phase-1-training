import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Restaurant } from '../models/restaurant.model';
import { MenuItem } from '../models/menu-item.model';
import { OrderDto } from '../models/order.model';

@Injectable({ providedIn: 'root' })
export class RestaurantService {
  private apiUrl = '/api/Restaurant';
  constructor(private http: HttpClient) {}
  getProfile(): Observable<Restaurant> { return this.http.get<Restaurant>(`${this.apiUrl}/profile`); }
  updateProfile(data: Partial<Restaurant>): Observable<Restaurant> { return this.http.put<Restaurant>(`${this.apiUrl}/profile`, data); }
  getMenu(): Observable<MenuItem[]> { return this.http.get<MenuItem[]>(`${this.apiUrl}/menu`); }
  addMenuItem(item: Partial<MenuItem>): Observable<MenuItem> { return this.http.post<MenuItem>(`${this.apiUrl}/menu`, item); }
  updateMenuItem(id: number, item: Partial<MenuItem>): Observable<MenuItem> { return this.http.put<MenuItem>(`${this.apiUrl}/menu/${id}`, item); }
  deleteMenuItem(id: number): Observable<void> { return this.http.delete<void>(`${this.apiUrl}/menu/${id}`); }
  getAllRestaurants(): Observable<Restaurant[]> { return this.http.get<Restaurant[]>(`${this.apiUrl}/AllRestaurant`); }
  getOrders(): Observable<OrderDto[]> { return this.http.get<OrderDto[]>(`${this.apiUrl}/orders`); }
  updateOrderStatus(id: number, status: string): Observable<OrderDto> { return this.http.put<OrderDto>(`${this.apiUrl}/orders/${id}/status`, { status }); }
}
