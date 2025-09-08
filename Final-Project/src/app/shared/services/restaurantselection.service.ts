import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class RestaurantSelectionService {
  private restaurantId: number | null = null;

  setRestaurantId(id: number) {
    this.restaurantId = id;
  }

  
  getRestaurantId(): number | null {
    return this.restaurantId;
  }
}
