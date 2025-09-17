import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MenuItem } from '../models/menu-item.model';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class MenuItemService {
  private apiUrl = '/api/MenuItem';
  constructor(private http: HttpClient) {}
  getAll(): Observable<MenuItem[]> { return this.http.get<MenuItem[]>(this.apiUrl); }
  getById(id: number) { return this.http.get<MenuItem>(`${this.apiUrl}/${id}`); }
  create(item: Partial<MenuItem>) { return this.http.post<MenuItem>(this.apiUrl, item); }
  update(id: number, item: Partial<MenuItem>) { return this.http.put<MenuItem>(`${this.apiUrl}/${id}`, item); }
  delete(id: number) { return this.http.delete<void>(`${this.apiUrl}/${id}`); }
}
