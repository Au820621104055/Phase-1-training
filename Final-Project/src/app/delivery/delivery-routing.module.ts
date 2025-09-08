import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { OrderStatusComponent } from './order-status/order-status.component';

const routes: Routes = [
  { path: '', component: DashboardComponent },
  {path:'dashboard', component:DashboardComponent},
  { path: 'order-status', component: OrderStatusComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DeliveryRoutingModule { }
