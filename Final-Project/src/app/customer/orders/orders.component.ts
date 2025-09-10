import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { CustomerService } from 'src/app/shared/services/customer.service';
import { orderReponce } from 'src/app/shared/models/orderResponce.interface';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.css']
})
export class OrdersComponent implements OnInit {
  orders: orderReponce[] = [];
  private orderApi = 'https://localhost:7279/api/Order/my-orders'; 

  constructor(private http: HttpClient, private service: CustomerService) {}

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

 
  cancel(order: orderReponce) {
    if (order.status !== 'Pending') {
      alert('This order is already processed and cannot be cancelled!');
      return;
    }

    const token = localStorage.getItem('authToken');
    const headers = token ? new HttpHeaders({ 'Authorization': `Bearer ${token}` }) : undefined;

   
    this.http.put(`${this.orderApi}/${order.orderId}/status`, { status: 'Cancelled' }, { headers }).subscribe({
      next: (res: any) => {
         
        order.status = 'Cancelled';
        if (order.payment && order.payment.amount) {
        }
        alert('Order cancelled successfully! Amount refunded.');
      },
      error: (err) => console.error('Failed to cancel order', err)
    });
  }
}
