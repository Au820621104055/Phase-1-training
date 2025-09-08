import { Component, OnInit } from '@angular/core';
import { DeliveryService } from '../../shared/services/delivery.service';
import { Order } from '../../shared/models/order.interface';

@Component({
  selector: 'app-order-status',
  templateUrl: './order-status.component.html',
  styleUrls: ['./order-status.component.css']
})
export class OrderStatusComponent implements OnInit {
  assignedOrders: Order[] = [];

  constructor(private deliveryService: DeliveryService) { }

  ngOnInit(): void {
    this.loadAssignedOrders();
  }

  /** Load assigned orders */
  loadAssignedOrders() {
    this.deliveryService.getAssignedOrders().subscribe({
      next: orders => this.assignedOrders = orders,
      error: err => console.error(err)
    });
  }

  /** Update delivery status */
  updateStatus(orderId: number, status: string) {
    this.deliveryService.updateOrderStatus(orderId, status).subscribe({
      next: () => this.loadAssignedOrders(),
      error: err => console.error(err)
    });
  }
}
