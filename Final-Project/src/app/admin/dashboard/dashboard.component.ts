import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { orderReponce } from 'src/app/shared/models/orderResponce.interface';
import { Restaurant } from 'src/app/shared/models/restaurant.interface';
import { User } from 'src/app/shared/models/user.interface';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  users: User[] = [];
  restaurants: Restaurant[] = [];
  orders: orderReponce[] = [];
  private apiUrl = 'https://localhost:7279/api/Admin';

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.loadUsers();
    this.loadRestaurants();
    this.loadOrders();
  }


  loadUsers() {
    this.http.get<any[]>(`${this.apiUrl}/users`).subscribe({
      next: (data) => this.users = data,
      error: (err) => console.error('Error loading users:', err)
    });
  }


  loadRestaurants() {
    this.http.get<any[]>(`${this.apiUrl}/dashboard/restaurants`).subscribe({
      next: (data) => this.restaurants = data,
      error: (err) => console.error('Error loading restaurants:', err)
    });
  }

  loadOrders() {
    this.http.get<any[]>(`https://localhost:7279/api/Admin/dashboard/orders`).subscribe({
      next: (data) => this.orders = data,
      error: (err) => console.error('Error loading orders:', err)
    });
  }

  updateUserStatus(userId: number, newStatus: string) {
    const isActive = newStatus === 'Active'; 

    this.http.put(
      `${this.apiUrl}/users/${userId}/status?isActive=${isActive}`,
      {},
    ).subscribe({
      next: () => {
        alert('Status updated successfully!');
        this.loadUsers(); 
      },
      error: (err) => console.error('Error updating status:', err)
    });
  }

updateRestaurantStatus(restaurantId: number, newStatus: string) {
  console.log('Updating restaurant ID:', restaurantId);
  this.http.patch(
    `${this.apiUrl}/restaurants/${restaurantId}/status?status=${newStatus}`,
    {}
  ).subscribe({
    next: () => {
      alert('Restaurant status updated successfully!');
      this.loadRestaurants();
    },
    error: (err) => console.error('Error updating restaurant status:', err)
  });
}
}
