import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Restaurant } from '../models/restaurant.model';
import { MenuItem } from '../models/menu-item.model';
import { Observable } from 'rxjs';
@Injectable({ providedIn: 'root' })
export class UserService { private apiUrl = '/api/User'; constructor(private http: HttpClient) {} getUsers(): Observable<any> { return this.http.get(this.apiUrl); } getById(id: number) { return this.http.get(`${this.apiUrl}/${id}`); } getByEmail(email: string) { return this.http.get(`${this.apiUrl}/email/${email}`); } getRestaurants(): Observable<Restaurant[]> { return this.http.get<Restaurant[]>(`${this.apiUrl}/restaurants`); } getRestaurantMenu(id: number): Observable<MenuItem[]> { return this.http.get<MenuItem[]>(`${this.apiUrl}/restaurant/${id}/menu`); } placeOrder(payload: any) { return this.http.post(`${this.apiUrl}/order`, payload); } makePayment(payload: any) { return this.http.post(`${this.apiUrl}/payment`, payload); } getOrderStatus(id: number) { return this.http.get(`${this.apiUrl}/order/${id}/status`); } }
