import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MenuItem } from 'src/app/shared/models/menuitem.interface';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {
  cart: { item: MenuItem; qty: number }[] = [];

  constructor(private router: Router) {}

  ngOnInit(): void {
    this.loadCart();
  }

  loadCart(): void {
    const saved = localStorage.getItem('cart');
    this.cart = saved ? JSON.parse(saved) : [];
  }

  increaseQty(index: number): void {
    this.cart[index].qty++;
    this.saveCart();
  }

  decreaseQty(index: number): void {
    if (this.cart[index].qty > 1) {
      this.cart[index].qty--;
    } else {
      this.removeItem(index);
    }
    this.saveCart();
  }

  removeItem(index: number): void {
    this.cart.splice(index, 1);
    this.saveCart();
  }

  getTotal(): number {
    return this.cart.reduce((sum, c) => sum + c.item.price * c.qty, 0);
  }

  checkout(): void {
    this.router.navigate(['/customer/checkout']);
  }

  private saveCart(): void {
    localStorage.setItem('cart', JSON.stringify(this.cart));
  }
}
