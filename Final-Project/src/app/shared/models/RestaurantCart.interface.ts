import { MenuItem } from "./menuitem.interface";

interface RestaurantCart {
  restaurantId: number;
  restaurantName: string;
  items: { item: MenuItem; qty: number }[];
}