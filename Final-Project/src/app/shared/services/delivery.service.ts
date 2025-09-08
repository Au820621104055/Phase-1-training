import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Order } from '../models/order.interface';

@Injectable({
  providedIn: 'root'
})
export class DeliveryService {
  private apiUrl = 'https://localhost:7279/api/Delivery';

  constructor(private http: HttpClient) {}

  /** Helper to get headers with token */
  private getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem('authToken');
    return new HttpHeaders({ 'Authorization': `Bearer ${token}` });
  }

  /** Get orders assigned to this delivery staff */
  getAssignedOrders(): Observable<Order[]> {
    return this.http.get<Order[]>(`${this.apiUrl}/assigned-orders`, { headers: this.getAuthHeaders() });
  }
  

  /** Update delivery status of an order */
  updateOrderStatus(orderId: number, status: string): Observable<Order> {
    return this.http.put<Order>(
      `${this.apiUrl}/orders/${orderId}/status`,
      { status },
      { headers: this.getAuthHeaders() }
    );
  }
}
