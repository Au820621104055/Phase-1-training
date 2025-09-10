import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Restaurant } from '../models/restaurant.interface';
import { MenuItem } from '../models/menuitem.interface';
import { Order } from '../models/order.interface';
import { orderReponce } from '../models/orderResponce.interface';

@Injectable({
  providedIn: 'root'
})
export class RestaurantService {
  private apiUrl = 'https://localhost:7279/api/Restaurant';
  private userApiUrl = 'https://localhost:7279/api/User';
  private orderApi = 'https://localhost:7279/api/Order';

  constructor(private http: HttpClient) {}

  private getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem('authToken'); 
    return new HttpHeaders({ 'Authorization': `Bearer ${token}` });
  }

 
  getProfile(): Observable<Restaurant> {
    return this.http.get<Restaurant>(`${this.apiUrl}/profile`, { headers: this.getAuthHeaders() });
  }

  updateProfile(data: Partial<Restaurant>): Observable<Restaurant> {
    return this.http.put<Restaurant>(`${this.apiUrl}/profile`, data, { headers: this.getAuthHeaders() });
  }

  getMenu(): Observable<MenuItem[]> {
    return this.http.get<MenuItem[]>(`${this.apiUrl}/menu`, { headers: this.getAuthHeaders() });
  }

  addMenuItem(item: Partial<MenuItem>): Observable<MenuItem> {
    return this.http.post<MenuItem>(`${this.apiUrl}/menu`, item, { headers: this.getAuthHeaders() });
  }

  updateMenuItem(id: number, item: Partial<MenuItem>): Observable<MenuItem> {
    return this.http.put<MenuItem>(`${this.apiUrl}/menu/${id}`, item, { headers: this.getAuthHeaders() });
  }

  deleteMenuItem(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/menu/${id}`, { headers: this.getAuthHeaders() });
  }

  getAllRestaurants(): Observable<Restaurant[]> {
    return this.http.get<Restaurant[]>(`${this.apiUrl}/AllRestaurant`); 
  }

  getRestaurantMenu(restaurantId: number): Observable<MenuItem[]> {
    return this.http.get<MenuItem[]>(`${this.userApiUrl}/restaurant/${restaurantId}/menu`, { headers: this.getAuthHeaders() });
  }

  getOrders(): Observable<Order[]> {
    return this.http.get<Order[]>(`${this.apiUrl}/orders`, { headers: this.getAuthHeaders() });
  }

getOrdersByRestaurant(id: number): Observable<orderReponce[]> {
  return this.http.get<orderReponce[]>(`${this.orderApi}/${id}`, { headers: this.getAuthHeaders() });
}

  addRestaurant(restaurant: any) {
    return this.http.post(`${this.apiUrl}/addrestaurant`, restaurant);
  }

  getOrdersByuser(id: number): Observable<orderReponce[]> {
    return this.http.get<orderReponce[]>(`${this.apiUrl}/orders/${id}`, { headers: this.getAuthHeaders() });
  }

getMenuByRestaurant(id: number): Observable<MenuItem[]> {
  return this.http.get<MenuItem[]>(`${this.userApiUrl}/restaurant/${id}/menu`, { headers: this.getAuthHeaders() });
}
  getMyRestaurants(): Observable<Restaurant[]> {
    return this.http.get<Restaurant[]>(`${this.apiUrl}/MyRestaurants`, { headers: this.getAuthHeaders() });
  }

  updateOrderStatus(id: number, status: string): Observable<Order> {
    return this.http.put<Order>(`${this.apiUrl}/orders/${id}/status`, { status }, { headers: this.getAuthHeaders() });
  }

  submitRestaurantForValidation(restaurant: Restaurant): Observable<Restaurant> {
    return this.http.post<Restaurant>(`${this.apiUrl}/submit-for-validation`, restaurant, { headers: this.getAuthHeaders() });
  }

  getPendingRestaurants(): Observable<Restaurant[]> {
    return this.http.get<Restaurant[]>(`${this.apiUrl}/pending`, { headers: this.getAuthHeaders() });
  }


  updateRestaurantStatus(id: number, status: 'Approved' | 'Rejected'): Observable<Restaurant> {
    return this.http.patch<Restaurant>(`${this.apiUrl}/${id}/status`, { status }, { headers: this.getAuthHeaders() });
  }
}
