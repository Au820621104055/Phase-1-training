import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DeliveryRoutingModule } from './delivery-routing.module';
import { DashboardComponent } from './dashboard/dashboard.component';
import { OrderStatusComponent } from './order-status/order-status.component';


@NgModule({
  declarations: [
    DashboardComponent,
    OrderStatusComponent
  ],
  imports: [
    CommonModule,
    DeliveryRoutingModule,
    FormsModule
  ]
})
export class DeliveryModule { }
