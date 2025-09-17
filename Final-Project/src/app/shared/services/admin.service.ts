import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../models/user.model';
@Injectable({ providedIn: 'root' })
export class AdminService { private apiUrl = '/api/Admin'; private userApi = '/api/User'; constructor(private http: HttpClient) {} getDashboardOrders(): Observable<any> { return this.http.get(`${this.apiUrl}/dashboard/orders`); } getDashboardRestaurants(): Observable<any> { return this.http.get(`${this.apiUrl}/dashboard/restaurants`); } getUsers(): Observable<User[]> { return this.http.get<User[]>(`${this.userApi}`); } updateUserStatus(id: number, status: string) { return this.http.put(`${this.userApi}/${id}/status`, { status }); } }
