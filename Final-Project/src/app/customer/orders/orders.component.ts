import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { CustomerService } from 'src/app/shared/services/customer.service';
import { Order } from 'src/app/shared/models/order.interface';
import { orderReponce } from 'src/app/shared/models/orderResponce.interface';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.css']
})
export class OrdersComponent implements OnInit {
  orders: Order[] = [];
  private orderApi = 'https://localhost:7279/api/Order/my-orders'; 

  constructor(private http: HttpClient,private service:CustomerService) {}

  ngOnInit(): void {
    const token = localStorage.getItem('authToken');
    const headers = token ? new HttpHeaders({ 'Authorization': `Bearer ${token}` }) : undefined;

    this.http.get<orderReponce[]>(this.orderApi, { headers }).subscribe({
    next: (res) => {
      this.orders = res;
      this.service.setOrders(res);   
    },
    error: (err) => console.error('Failed to fetch orders', err)
  });
  }
}
