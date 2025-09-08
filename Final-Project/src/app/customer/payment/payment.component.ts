import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { MenuItem } from 'src/app/shared/models/menuitem.interface';

@Component({
  selector: 'app-payment',
  templateUrl: './payment.component.html',
  styleUrls: ['./payment.component.css']
})
export class PaymentComponent implements OnInit {
  cart: { item: MenuItem; qty: number }[] = [];
  total: number = 0;
  address: string = '';
  specialInstructions: string = '';
  paymentMethod: string = 'COD';

  private orderApi = 'https://localhost:7279/api/User/order';
  private paymentApi = 'https://localhost:7279/api/User/payment';

  constructor(private router: Router, private http: HttpClient) {}

  ngOnInit(): void {
    const saved = localStorage.getItem('checkoutCart');
    if (saved) {
      const data = JSON.parse(saved);
      this.cart = data.cart;
      this.total = data.total;
      this.address = data.address;
      this.specialInstructions = data.specialInstructions;
    } else {
      alert('No order found. Please go back to checkout.');
      this.router.navigate(['/customer/checkout']);
    }
  }

  pay(): void {
    const token = localStorage.getItem('authToken');
    if (!token) {
      alert('Please login first!');
      this.router.navigate(['/auth/login']);
      return;
    }

    const headers = { 'Authorization': `Bearer ${token}` };

    const orderPayload = {
      RestaurantId: this.cart[0]?.item.restaurantId,
      Items: this.cart.map(c => ({ menuItemId: c.item.menuItemId, quantity: c.qty })),
      SpecialInstructions: this.specialInstructions,
      Address: this.address
    };

    if (this.paymentMethod === 'COD') {
      this.http.post(this.orderApi, orderPayload, { headers }).subscribe({
        next: () => {
          alert('Order placed successfully with COD!');
          localStorage.removeItem('checkoutCart');
          localStorage.removeItem('cart');
          this.router.navigate(['/customer']);
        },
        error: () => alert('Failed to place order!')
      });
    } else {
 
      this.http.post(this.orderApi, orderPayload, { headers }).subscribe({
        next: (res: any) => {
          const orderId = res.orderId;
          const paymentPayload = { orderId, paymentMethod: this.paymentMethod };

          this.http.post(this.paymentApi, paymentPayload, { headers }).subscribe({
            next: () => {
              alert(`${this.paymentMethod} payment successful!`);
              localStorage.removeItem('checkoutCart');
              localStorage.removeItem('cart');
              this.router.navigate(['/customer/orders']);
            },
            error: () => alert('Payment failed!')
          });
        },
        error: () => alert('Order failed!')
      });
    }
  }
}
