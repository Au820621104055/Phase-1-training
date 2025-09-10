import { Component, OnInit } from '@angular/core';
import { DeliveryService, DeliveryOrder } from '../../shared/services/delivery.service';

@Component({
  selector: 'app-order-status',
  templateUrl: './order-status.component.html',
  styleUrls: ['./order-status.component.css']
})
export class OrderStatusComponent implements OnInit {
  assignedOrders: DeliveryOrder[] = [];
  statusOptions = ['Picked Up', 'On The Way', 'Delivered', 'Cancelled','	On The Way']; 

  constructor(private deliveryService: DeliveryService) { }

  ngOnInit(): void {
    this.loadAssignedOrders();
  }

  loadAssignedOrders() {
    this.deliveryService.getAssignedOrders().subscribe({
      next: orders => this.assignedOrders = orders,
      error: err => console.error('Error loading assigned orders:', err)
    });
  }

  updateStatus(orderId: number, status: string) {
    this.deliveryService.updateOrderStatus(orderId, status).subscribe({
      next: () => this.loadAssignedOrders(),
      error: err => console.error('Error updating status:', err)
    });
  }
}
