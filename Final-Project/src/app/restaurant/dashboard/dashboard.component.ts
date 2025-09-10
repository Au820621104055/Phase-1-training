import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { RestaurantService } from '../../shared/services/restaurant.service';
import { Restaurant } from 'src/app/shared/models/restaurant.interface';
import { MenuItem } from 'src/app/shared/models/menuitem.interface';
import { orderReponce } from 'src/app/shared/models/orderResponce.interface';
import { AuthService } from 'src/app/shared/services/auth.service';

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
  restaurantForm: FormGroup;
  showAddRestaurantForm = false;   

  constructor(private restaurantService: RestaurantService, private fb: FormBuilder ,private service:AuthService) {

    this.menuForm = this.fb.group({
      name: ['', Validators.required],
      description: [''],
      price: [0, [Validators.required, Validators.min(1)]],
      restaurantId: [this.selectedRestaurant?.restaurantId],
      isAvailable: [true]
    });

    this.restaurantForm = this.fb.group({
      name: ['', Validators.required],
    address: ['', Validators.required],       
    phoneNumber: ['', Validators.required],   
    cuisineType: [''],
    ownerId: [this.service.getUserId(), Validators.required]
    });
  }

  ngOnInit(): void {
    this.loadOwnedRestaurants();
  }

loadOwnedRestaurants() {
  this.restaurantService.getMyRestaurants().subscribe({
    next: res => this.ownedRestaurants = res,
    error: err => console.error(err)
  });
}

addRestaurant() {
  if (this.restaurantForm.invalid) {
    alert('Please fill all required fields.');
    return;
  }

  const restaurantData: Restaurant = {
    ...this.restaurantForm.value,
    status: 'Pending'   
  };

  this.restaurantService.submitRestaurantForValidation(restaurantData).subscribe({
    next: () => {
      alert('Restaurant submitted for validation! Admin will approve it shortly.');
      this.restaurantForm.reset();
      this.showAddRestaurantForm = false;
      this.loadOwnedRestaurants(); // optionally show pending restaurants
    },
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
  this.restaurantService.getMenuByRestaurant(this.selectedRestaurant.restaurantId).subscribe({
    next: res => this.menuItems = res,
    error: err => console.error(err)
  });
}

addOrUpdateMenu() {
  if (!this.selectedRestaurant) return;

  const itemData = {
    ...this.menuForm.value,
    restaurantId: this.selectedRestaurant.restaurantId  
  };

  console.log("Payload to API:", itemData);

  if (this.editingMenuItemId) {
    this.restaurantService.updateMenuItem(this.selectedRestaurant.restaurantId, itemData)
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
  debugger
  if (!this.selectedRestaurant) return;
  const resid = this.selectedRestaurant.restaurantId;
  this.restaurantService.getOrdersByRestaurant(resid).subscribe({
    next: res => this.orders = res,
    error: err => console.error(err)
  });
}

  updateOrderStatus(orderId: number, status: string) {
    this.restaurantService.updateOrderStatus(orderId, status).subscribe({
      next: () => this.loadOrders(),
      error: err => console.error(err)
    });
  }

  onStatusChange(orderId: number, event: Event) {
  const selectElement = event.target as HTMLSelectElement;
  const status = selectElement.value;
  this.updateOrderStatus(orderId, status);
}
}
