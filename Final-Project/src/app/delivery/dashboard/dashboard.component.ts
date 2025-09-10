import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/shared/services/auth.service';
import { DeliveryService, DeliveryOrder } from '../../shared/services/delivery.service';

@Component({
  selector: 'app-order-available',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  orders: DeliveryOrder[] = [];
  assignedOrders: DeliveryOrder[] = [];
  isAvailableOrders = true; 
  statusOptions = ['Picked Up', 'On The Way', 'Delivered', 'Cancelled']; 

  constructor(private deliveryService: DeliveryService,private service:AuthService) { }

  ngOnInit(): void {
    this.loadAvailableOrders();
  }

  loadAvailableOrders() {
    this.deliveryService.getAvailableOrders().subscribe({
      next: orders => this.orders = orders,
      error: err => console.error(err)
    });
  }

  loadAssignedOrders() {
    this.deliveryService.getAssignedOrders().subscribe({
      next: orders => this.assignedOrders = orders,
      error: err => console.error(err)
    });
  }

  showAvailableOrders() {
    this.isAvailableOrders = true;
    this.loadAvailableOrders();
  }

  showAssignedOrders() {
    this.isAvailableOrders = false;
    this.loadAssignedOrders();
  }

  accept(orderId: number) {
    
    const deliveryPersonId = this.service.getUserId(); 
    this.deliveryService.acceptOrder(orderId, deliveryPersonId).subscribe({
      next: () => this.loadAvailableOrders(),
      error: err => console.error(err)
    });
  }

  reject(orderId: number) {
    this.deliveryService.rejectOrder(orderId).subscribe({
      next: () => this.loadAvailableOrders(),
      error: err => console.error(err)
    });
  }

  updateStatus(orderId: number, status: string) {
    this.deliveryService.updateOrderStatus(orderId, status).subscribe({
      next: () => this.loadAssignedOrders(),
      error: err => console.error(err)
    });
  }
}
