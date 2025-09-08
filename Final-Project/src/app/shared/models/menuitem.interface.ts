import { Restaurant } from './restaurant.interface';
import { OrderItem } from './orderitem.interface';

export interface MenuItem {
  menuItemId: number;
  name: string;
  description: string;
  price: number;
  isAvailable: boolean;
  restaurantId: number;
  restaurant: Restaurant;
  orderItems: OrderItem[];
  image?:string;
}