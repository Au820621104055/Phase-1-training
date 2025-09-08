import { Component } from '@angular/core';
import { DeliveryService } from 'src/app/shared/services/delivery.service';
import { AuthService } from 'src/app/shared/services/auth.service';
import { Order } from 'src/app/shared/models/order.interface';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent {
  public staffname: string = ''; 
  public deliveredOrders:Order[] = [];

  constructor(private authservice: AuthService ,private deliveryService:DeliveryService) { }

  ngOnInit(): void {
    this.staffname = this.authservice.getFullName() ?? ''; 

    this.deliveryService.getAssignedOrders().subscribe({
    next: res => this.deliveredOrders = res.filter(order => order.deliveryStatus === 'Delivered'),
    error: err => console.error(err)
  });
  }

}

