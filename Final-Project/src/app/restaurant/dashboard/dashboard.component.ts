
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { RestaurantService } from '../../shared/services/restaurant.service';
import { Restaurant } from 'src/app/shared/models/restaurant.interface';
import { MenuItem } from 'src/app/shared/models/menuitem.interface';
import { Order } from 'src/app/shared/models/order.interface';
import { orderReponce } from 'src/app/shared/models/orderResponce.interface';
import { Route } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  ownedRestaurants: Restaurant[] = [];
  selectedRestaurant: Restaurant | null = null;
  menuItems: MenuItem[] = [];
  orders: orderReponce[] = [];
  menuForm: FormGroup;
  editingMenuItemId: number | null = null;
  public resid: number | undefined = this.selectedRestaurant?.restaurantId;


  constructor(private restaurantService: RestaurantService, private fb: FormBuilder) {
    this.menuForm = this.fb.group({
      name: ['', Validators.required],
      description: [''],
      price: [0, [Validators.required, Validators.min(1)]],
      isAvailable: [true]
    });
  }

  ngOnInit(): void {
    this.loadOwnedRestaurants();
  }

  loadOwnedRestaurants() {
    this.restaurantService.getAllRestaurants().subscribe({
      next: res => this.ownedRestaurants = res,
      error: err => console.error(err)
    });
  }

  selectRestaurant(restaurant: Restaurant) {
    this.selectedRestaurant = restaurant;
    this.loadMenu();
    this.loadOrders();
  }

  loadMenu() {
    if (!this.selectedRestaurant) return;
    this.restaurantService.getMenu().subscribe({
      next: res => this.menuItems = res,
      error: err => console.error(err)
    });
  }

  addOrUpdateMenu() {
    if (!this.selectedRestaurant) return;
    const itemData = this.menuForm.value;
    if (this.editingMenuItemId) {
      this.restaurantService.updateMenuItem(this.editingMenuItemId, itemData)
        .subscribe({
          next: () => { this.loadMenu(); this.resetMenuForm(); },
          error: err => console.error(err)
        });
    } else {
      this.restaurantService.addMenuItem(itemData)
        .subscribe({
          next: () => { this.loadMenu(); this.resetMenuForm(); },
          error: err => console.error(err)
        });
    }
  }

  editMenuItem(item: MenuItem) {
    this.editingMenuItemId = item.menuItemId;
    this.menuForm.setValue({
      name: item.name,
      description: item.description,
      price: item.price,
      isAvailable: item.isAvailable
    });
  }

  deleteMenuItem(id: number) {
    if (!this.selectedRestaurant) return;
    if (!confirm('Are you sure you want to delete this item?')) return;
    this.restaurantService.deleteMenuItem(id).subscribe({
      next: () => this.loadMenu(),
      error: err => console.error(err)
    });
  }

  resetMenuForm() {
    this.menuForm.reset({ isAvailable: true, price: 0 });
    this.editingMenuItemId = null;
  }

loadOrders() {
    if (!this.selectedRestaurant) return;
    const resid = this.selectedRestaurant.restaurantId;
    this.restaurantService.getOrdersById(resid).subscribe({
      next: res => {
        this.orders = res;
      },
      error: err => console.error(err)
    });
    //this.router.navigate(['/restaurant/order${this.selectedRestaurant}']);
    
  }

  updateOrderStatus(orderId: number, status: string) {
    this.restaurantService.updateOrderStatus(orderId, status).subscribe({
      next: () => this.loadOrders(),
      error: err => console.error(err)
    });
  }
}

