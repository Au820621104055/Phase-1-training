import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MenuItem } from 'src/app/shared/models/menuitem.interface';

interface RestaurantCart {
  restaurantId: number;
  restaurantName: string;
  items: { item: MenuItem; qty: number }[];
}

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {
  carts: RestaurantCart[] = [];  

  constructor(private router: Router) {}

  ngOnInit(): void {
    this.loadCart();
  }

  loadCart(): void {
    const saved = localStorage.getItem('cart');
    const flatCart: { item: MenuItem; qty: number }[] = saved ? JSON.parse(saved) : [];


    const grouped: { [key: number]: RestaurantCart } = {};
    flatCart.forEach(c => {
      const rid = c.item.restaurantId;
      if (!grouped[rid]) {
        grouped[rid] = {
          restaurantId: rid,
          restaurantName: `Restaurant Id - ${c.item.restaurantId}`, 
          items: []
        };
      }
      grouped[rid].items.push(c);
    });

    this.carts = Object.values(grouped);
  }

  increaseQty(cartIndex: number, itemIndex: number): void {
    this.carts[cartIndex].items[itemIndex].qty++;
    this.saveCart();
  }

  decreaseQty(cartIndex: number, itemIndex: number): void {
    const item = this.carts[cartIndex].items[itemIndex];
    if (item.qty > 1) {
      item.qty--;
    } else {
      this.removeItem(cartIndex, itemIndex);
    }
    this.saveCart();
  }

  removeItem(cartIndex: number, itemIndex: number): void {
    this.carts[cartIndex].items.splice(itemIndex, 1);
   
    if (this.carts[cartIndex].items.length === 0) {
      this.carts.splice(cartIndex, 1);
    }
    this.saveCart();
  }

  getTotal(cart: RestaurantCart): number {
    return cart.items.reduce((sum, c) => sum + c.item.price * c.qty, 0);
  }

  checkoutRestaurant(cartIndex: number): void {
    const cartToCheckout = this.carts[cartIndex];

    localStorage.setItem('checkoutCart', JSON.stringify({
      cart: cartToCheckout.items,
      total: this.getTotal(cartToCheckout),
      restaurantId: cartToCheckout.restaurantId,
      restaurantName: cartToCheckout.restaurantName
    }));

    this.router.navigate(['/customer/checkout']);
  }

  private saveCart(): void {
    const flatCart: { item: MenuItem; qty: number }[] = [];
    this.carts.forEach(c => {
      flatCart.push(...c.items);
    });
    localStorage.setItem('cart', JSON.stringify(flatCart));
  }
}
