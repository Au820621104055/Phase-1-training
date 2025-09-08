import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { CustomerRoutingModule } from './customer-routing.module';
import { DashboardComponent } from './dashboard/dashboard.component';
import { RestaurantListComponent } from './restaurant-list/restaurant-list.component';
import { RestaurantDetailsComponent } from './restaurant-details/restaurant-details.component';
import { CartComponent } from './cart/cart.component';
import { CheckoutComponent } from './checkout/checkout.component';
import { OrdersComponent } from './orders/orders.component';

@NgModule({
  declarations: [
    DashboardComponent,
    RestaurantListComponent,
    RestaurantDetailsComponent,
    CartComponent,
    CheckoutComponent,
    OrdersComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    CustomerRoutingModule
  ]
})
export class CustomerModule {}
