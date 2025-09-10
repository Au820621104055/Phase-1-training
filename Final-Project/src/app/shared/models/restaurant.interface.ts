import { MenuItem } from './menuitem.interface';
import { User } from './user.interface';

export interface Restaurant {
  restaurantId: number;
  name: string;
  address: string;
  phoneNumber: string;
  cuisineType: string;
  ownerId: number;
  owner: User;
  menuItems: MenuItem[];
  image?:string;
  status?: string;
}