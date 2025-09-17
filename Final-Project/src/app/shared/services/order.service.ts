import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { OrderDto } from '../models/order.model';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class OrderService {
  private apiUrl = '/api/Order';
  private userApi = '/api/User';
  constructor(private http: HttpClient) {}
  getAll(): Observable<OrderDto[]> { return this.http.get<OrderDto[]>(this.apiUrl); }
  getById(id: number): Observable<OrderDto> { return this.http.get<OrderDto>(`${this.apiUrl}/${id}`); }
  create(order: OrderDto): Observable<any> { return this.http.post(this.apiUrl, order); }
  update(id: number, order: Partial<OrderDto>): Observable<OrderDto> { return this.http.put<OrderDto>(`${this.apiUrl}/${id}`, order); }
  delete(id: number): Observable<void> { return this.http.delete<void>(`${this.apiUrl}/${id}`); }
  getCustomerOrderStatus(orderId: number) { return this.http.get(`${this.userApi}/order/${orderId}/status`); }
  placeUserOrder(payload: any) { return this.http.post(`${this.userApi}/order`, payload); }
}
