import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { RestaurantService } from '../../shared/services/restaurant.service';
import { MenuItem } from 'src/app/shared/models/menuitem.interface';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-menu',
  templateUrl: './menu-management.component.html'
})
export class MenuComponent implements OnChanges {
  @Input() restaurantId!: number;

  menuItems: MenuItem[] = [];
  menuForm: FormGroup;
  editingMenuItemId: number | null = null;

  constructor(private service: RestaurantService, private fb: FormBuilder) {
    this.menuForm = this.fb.group({
      name: ['', Validators.required],
      description: [''],
      price: [0, [Validators.required, Validators.min(1)]],
      isAvailable: [true]
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['restaurantId'] && this.restaurantId) {
      this.loadMenu();
    }
  }

  loadMenu() {
    debugger
    this.service.getMenuByRestaurant(this.restaurantId).subscribe({
      next: res => this.menuItems = res,
      error: err => console.error(err)
    });
  }

addOrUpdateMenu() {
  const data = {
    ...this.menuForm.value,
    restaurantId: this.restaurantId, 
    quantity: 0
  };

  if (this.editingMenuItemId) {
    this.service.updateMenuItem(this.editingMenuItemId, data).subscribe({
      next: () => { this.loadMenu(); this.resetForm(); },
      error: err => console.error(err)
    });
  } else {
    this.service.addMenuItem(data).subscribe({
      next: () => { this.loadMenu(); this.resetForm(); },
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
    if (!confirm('Delete this item?')) return;
    this.service.deleteMenuItem(id).subscribe({
      next: () => this.loadMenu(),
      error: err => console.error(err)
    });
  }

  resetForm() {
    this.menuForm.reset({ isAvailable: true, price: 0 });
    this.editingMenuItemId = null;
  }
}
