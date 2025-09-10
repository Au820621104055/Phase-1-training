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
  paymentMethod: string = 'COD'; // Options: COD, CARD, UPI

  private orderApi = 'https://localhost:7279/api/User/order';

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

    if (!this.cart.length) {
      alert('Your cart is empty!');
      return;
    }

    const orderPayload = {
      RestaurantId: this.cart[0]?.item.restaurantId,
      Items: this.cart.map(c => ({ menuItemId: c.item.menuItemId, quantity: c.qty })),
      SpecialInstructions: this.specialInstructions,
      Address: this.address
    };

    console.log('Order Payload:', orderPayload);

    this.http.post(this.orderApi, orderPayload, { headers }).subscribe({
      next: (res: any) => {
 
        alert(`${this.paymentMethod === 'COD' ? 'Order placed successfully with COD!' : 'Payment successful!'}`);

 
        localStorage.removeItem('checkoutCart');
        localStorage.removeItem('cart');
 
        if (this.paymentMethod === 'COD') {
          this.router.navigate(['/customer']);
        } else {
          this.router.navigate(['/customer/orders']);
        }
      },
      error: (err) => {
        console.error('Order/Payment error:', err);
        alert('Failed to place order or payment!');
      }
    });
  }
}
