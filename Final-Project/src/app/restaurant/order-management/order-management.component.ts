import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { RestaurantService } from '../../shared/services/restaurant.service';
import { orderReponce } from 'src/app/shared/models/orderResponce.interface';

@Component({
  selector: 'app-orders',
  templateUrl: './order-management.component.html'
})
export class OrdersComponent implements OnChanges {
  @Input() restaurantId!: number;
  orders: orderReponce[] = [];

  constructor(private service: RestaurantService) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['restaurantId'] && this.restaurantId) {
      this.loadOrders();
    }
  }

  loadOrders() {
    this.service.getOrdersByRestaurant(this.restaurantId).subscribe({
      next: res => this.orders = res,
      error: err => console.error(err)
    });
  }

  updateOrderStatus(orderId: number, status: string) {
    this.service.updateOrderStatus(orderId, status).subscribe({
      next: () => this.loadOrders(),
      error: err => console.error(err)
    });
  }
}
