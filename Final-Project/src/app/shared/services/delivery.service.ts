import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { OrderDto } from '../models/order.model';
@Injectable({ providedIn: 'root' })
export class DeliveryService { private apiUrl = '/api/Delivery'; constructor(private http: HttpClient) {} getAssignedOrders(): Observable<OrderDto[]> { return this.http.get<OrderDto[]>(`${this.apiUrl}/assigned-orders`); } updateOrderStatus(id: number, status: string): Observable<OrderDto> { return this.http.put<OrderDto>(`${this.apiUrl}/orders/${id}/status`, { status }); } }
