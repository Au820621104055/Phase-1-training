import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { MenuItem } from 'src/app/shared/models/menuitem.interface';

@Component({
  selector: 'app-restaurant-details',
  templateUrl: './restaurant-details.component.html',
  styleUrls: ['./restaurant-details.component.css']
})
export class RestaurantDetailsComponent implements OnInit {
  restaurantId!: number;
  restaurantName: string = '';
  menu: MenuItem[] = [];
  cart: { item: MenuItem; qty: number }[] = [];

  private apiUrl = 'https://localhost:7279/api/User/restaurant';

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.restaurantId = Number(this.route.snapshot.paramMap.get('id'));
    this.loadMenu();
    this.loadCart();
  }

  loadMenu(): void {
  this.http.get<MenuItem[]>(`${this.apiUrl}/${this.restaurantId}/menu`).subscribe({
    next: (res) => {
      this.menu = res.map((item, index) => ({
        ...item,
        image: `/assets/images/menu/image${index + 1}.jpg` 
      }));
    },
    error: (err) => {
      console.error('Failed to load menu', err);
    }
  });
}

  loadCart(): void {
    const saved = localStorage.getItem('cart');
    this.cart = saved ? JSON.parse(saved) : [];
  }

  addToCart(item: MenuItem): void {
    const existing = this.cart.find(c => c.item.menuItemId === item.menuItemId);
    if (existing) {
      existing.qty += 1;
    } else {
      this.cart.push({ item, qty: 1 });
    }

    localStorage.setItem('cart', JSON.stringify(this.cart));
    // alert(`${item.name} added to cart!`);
  }

  goToCart(): void {
    this.router.navigate(['/customer/cart']);
  }
}
