import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CustomerService } from 'src/app/shared/services/customer.service';
import { AuthService } from 'src/app/shared/services/auth.service';
import { User } from 'src/app/shared/models/user.interface';
import { orderReponce } from 'src/app/shared/models/orderResponce.interface';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  customer: User | null = null;
  loading = true;
  recentOrders: orderReponce[] = [];

  constructor(
    private customerService: CustomerService,
    private auth: AuthService,
    private router: Router,
    private http: HttpClient,
    private service:CustomerService
  ) {}
  private orderApi = 'https://localhost:7279/api/Order/my-orders';

  ngOnInit(): void {
    const userId = Number(localStorage.getItem('userId'));
    if (!userId) {
      this.loading = false;
      return;
    }


    this.customerService.getProfile(userId).subscribe({
      next: (res) => {
        this.customer = res;
        this.loadOrders(userId);
        this.loading = false;
      },
      error: () => this.loading = false
    });
  }

  loadOrders(userId: number) {
    const token = localStorage.getItem('authToken');
        const headers = token ? new HttpHeaders({ 'Authorization': `Bearer ${token}` }) : undefined;
    
        this.http.get<orderReponce[]>(this.orderApi, { headers }).subscribe({
        next: (res) => {
          this.recentOrders = res;
          this.service.setOrders(res);   
        },
        error: (err) => console.error('Failed to fetch orders', err)
      });
  }

  goToCart() {
    this.router.navigate(['/customer/cart']);
  }

  goToRestaurants() {
    this.router.navigate(['/customer/restaurant-list']); 
  }

  goToOrders() {
    this.router.navigate(['/customer/orders']); 
  }

  logout() {
    this.auth.logout();
    this.router.navigate(['/auth/login']);
  }
}
