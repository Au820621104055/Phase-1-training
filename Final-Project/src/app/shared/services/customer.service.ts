import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../models/user.interface';
import { Restaurant } from '../models/restaurant.interface';
import { MenuItem } from '../models/menuitem.interface';
import { Order } from '../models/order.interface';
import { Payment } from '../models/payment.interface';
import { orderReponce } from '../models/orderResponce.interface';
import { deliveryorder } from '../models/deliveryorder.interface';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {
  private apiUrl = 'https://localhost:7279/api/User';
  private orderApi = 'https://localhost:7279/api/Order/my-orders'; 
  private baseUrl = 'http://localhost:7279/api/delivery';

  recentorder:Order[]=[];


  constructor(private http: HttpClient) {}

    setOrders(orders: Order[]) {
    this.recentorder=orders;
  }

    private getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem('authToken'); 
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }

  getProfile(userId: number): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/${userId}`);
  }

  getRestaurants(): Observable<Restaurant[]> {
    return this.http.get<Restaurant[]>(`${this.apiUrl}/restaurants`);
  }

  getMenu(restaurantId: number): Observable<MenuItem[]> {
    return this.http.get<MenuItem[]>(`${this.apiUrl}/restaurant/${restaurantId}/menu`);
  }

  placeOrder(order: any): Observable<Order> {
    return this.http.post<Order>(`${this.apiUrl}/order`, order);
  }

  makePayment(payment: any): Observable<Payment> {
    return this.http.post<Payment>(`${this.apiUrl}/payment`, payment);
  }

  cancelOrder(orderId: number) {
  const token = localStorage.getItem('authToken');
  const headers = token ? { 'Authorization': `Bearer ${token}` } : {};
  return this.http.put(`https://localhost:7279/api/Order/cancel/${orderId}`,{headers:this.getAuthHeaders()});
}

  trackOrder(orderId: number): Observable<string> {
    return this.http.get<string>(`${this.apiUrl}/order/${orderId}/status`);
  }

  getAvailableOrders(): Observable<deliveryorder[]> {
    return this.http.get<deliveryorder[]>(`${this.baseUrl}/available`);
  }

  acceptOrder(orderId: number, deliveryPersonId: number): Observable<any> {
    return this.http.post(`${this.baseUrl}/accept/${orderId}/${deliveryPersonId}`, {});
  }

  rejectOrder(orderId: number): Observable<any> {
    return this.http.post(`${this.baseUrl}/reject/${orderId}`, {});
  }
}
