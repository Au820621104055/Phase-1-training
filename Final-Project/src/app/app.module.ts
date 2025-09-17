import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { AuthModule } from './auth/auth.module';
import { AppComponent } from './app.component';
import { AuthInterceptor } from './shared/interceptors/auth.interceptor';

import { CustomerDashboardComponent } from './customer/dashboard/dashboard.component';
import { RestaurantDashboardComponent } from './restaurant/dashboard/dashboard.component';
import { AdminDashboardComponent } from './admin/dashboard/dashboard.component';
import { DeliveryDashboardComponent } from './delivery/dashboard/dashboard.component';

@NgModule({
  declarations: [
    AppComponent,
    CustomerDashboardComponent,
    RestaurantDashboardComponent,
    AdminDashboardComponent,
    DeliveryDashboardComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    AppRoutingModule,
    AuthModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
