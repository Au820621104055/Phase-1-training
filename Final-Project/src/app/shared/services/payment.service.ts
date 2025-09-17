import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Payment } from '../models/payment.model';
import { Observable } from 'rxjs';
@Injectable({ providedIn: 'root' })
export class PaymentService { private apiUrl = '/api/Payment'; constructor(private http: HttpClient) {} getAll(): Observable<Payment[]> { return this.http.get<Payment[]>(this.apiUrl); } getById(id: number): Observable<Payment> { return this.http.get<Payment>(`${this.apiUrl}/${id}`); } create(payment: Partial<Payment>) { return this.http.post<Payment>(this.apiUrl, payment); } update(id: number, payment: Partial<Payment>) { return this.http.put<Payment>(`${this.apiUrl}/${id}`, payment); } delete(id: number) { return this.http.delete<void>(`${this.apiUrl}/${id}`); } }
