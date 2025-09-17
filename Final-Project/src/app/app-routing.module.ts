import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { CustomerDashboardComponent } from './customer/dashboard/dashboard.component';
import { RestaurantDashboardComponent } from './restaurant/dashboard/dashboard.component';
import { AdminDashboardComponent } from './admin/dashboard/dashboard.component';
import { DeliveryDashboardComponent } from './delivery/dashboard/dashboard.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },

  { path: 'customer/dashboard', component: CustomerDashboardComponent },
  { path: 'restaurant/dashboard', component: RestaurantDashboardComponent },
  { path: 'admin/dashboard', component: AdminDashboardComponent },
  { path: 'delivery/dashboard', component: DeliveryDashboardComponent },

  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
