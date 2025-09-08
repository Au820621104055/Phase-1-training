import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MenuItem } from 'src/app/shared/models/menuitem.interface';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css']
})
export class CheckoutComponent implements OnInit {
  cart: { item: MenuItem; qty: number }[] = [];
  total: number = 0;
  address: string = '';
  specialInstructions: string = '';

  constructor(private router: Router) {}

  ngOnInit(): void {
    const saved = localStorage.getItem('cart');
    this.cart = saved ? JSON.parse(saved) : [];
    this.calculateTotal();
  }

  calculateTotal(): void {
    this.total = this.cart.reduce((sum, c) => sum + c.item.price * c.qty, 0);
  }

  proceedToPayment(): void {
    if (!this.cart.length) {
      alert('Your cart is empty!');
      return;
    }

    localStorage.setItem('checkoutCart', JSON.stringify({
      cart: this.cart,
      total: this.total,
      address: this.address,
      specialInstructions: this.specialInstructions
    }));

    this.router.navigate(['/customer/payment']);
  }
}
