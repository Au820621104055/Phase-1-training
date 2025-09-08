import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PaymentComponent } from './customer/payment/payment.component';
import { HomeComponent } from './customer/home/home.component';
import { AuthGuard } from './shared/guards/auth.guard';
import { RoleGuard } from './shared/guards/role.guard';
import { LoginComponent } from './auth/login/login.component';

const routes: Routes = [
  { path: '', component: LoginComponent },

  {
    path: 'auth',
    loadChildren: () =>
      import('./auth/auth.module').then(m => m.AuthModule)
  },

  {
    path: 'customer',
    loadChildren: () =>
      import('./customer/customer.module').then(m => m.CustomerModule),
    canActivate: [AuthGuard]
  },

  {
    path: 'restaurant',
    loadChildren: () =>
      import('./restaurant/restaurant.module').then(m => m.RestaurantModule),
    canActivate: [AuthGuard, RoleGuard],
    data: { roles: ['RestaurantOwner'] }
  },

  {
    path: 'delivery',
    loadChildren: () =>
      import('./delivery/delivery.module').then(m => m.DeliveryModule),
    canActivate: [AuthGuard, RoleGuard],
    data: { roles: ['DeliveryStaff'] }
  },

  {
    path: 'admin',
    loadChildren: () =>
      import('./admin/admin.module').then(m => m.AdminModule),
    canActivate: [AuthGuard, RoleGuard],
    data: { roles: ['Admin'] }
  },

  { path: 'customer/payment', component: PaymentComponent, canActivate: [AuthGuard] },


  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
