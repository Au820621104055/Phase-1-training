import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { RestaurantRoutingModule } from './restaurant-routing.module';
import { DashboardComponent } from './dashboard/dashboard.component';
import { FormsModule } from '@angular/forms';
import { MenuComponent } from './menu-management/menu-management.component';
import { OrdersComponent } from './order-management/order-management.component';


@NgModule({
  declarations: [
    DashboardComponent,
    MenuComponent,
    OrdersComponent
  ],
  imports: [
    CommonModule,
    RestaurantRoutingModule,
    ReactiveFormsModule,
    FormsModule
  ]
})
export class RestaurantModule { }
