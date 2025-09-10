import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface DeliveryOrder {
  orderId: number;
  orderDate: string;
  deliveryStatus: string;
  customerName: string;
  restaurantName: string;
}

@Injectable({
  providedIn: 'root'
})
export class DeliveryService {
  private baseUrl = 'https://localhost:7279/api/delivery';

  constructor(private http: HttpClient) {}


    private getAuthHeaders(): HttpHeaders {
      const token = localStorage.getItem('authToken'); 
      return new HttpHeaders({ 'Authorization': `Bearer ${token}` });
    }
    
  getAvailableOrders(): Observable<DeliveryOrder[]> {
    return this.http.get<DeliveryOrder[]>(`${this.baseUrl}/available`);
  }

  getAssignedOrders(): Observable<DeliveryOrder[]> {
    return this.http.get<DeliveryOrder[]>(`${this.baseUrl}/assigned-orders`,{headers:this.getAuthHeaders()});
  }

  acceptOrder(orderId: number, deliveryPersonId: number): Observable<any> {
    return this.http.post(`${this.baseUrl}/accept/${orderId}/${deliveryPersonId}`, {});
  }

updateOrderStatus(orderId: number, status: string) {
  return this.http.put(`${this.baseUrl}/orders/${orderId}/status`, { status },{headers:this.getAuthHeaders()});
}

  rejectOrder(orderId: number): Observable<any> {
    return this.http.post(`${this.baseUrl}/reject/${orderId}`, {});
  }
}
