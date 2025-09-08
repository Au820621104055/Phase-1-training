import { User } from './user.interface';
import { Restaurant } from './restaurant.interface';
import { OrderItem } from './orderitem.interface';
import { Payment } from './payment.interface';

export interface Order {
  orderId: number;
  customerId: number;
  customerName: string;
  restaurantId: number;
  restaurantName: string;
  orderDate: string;
  deliveryPersonId?: number | null;
  deliveryPersonName?: string | null;
  deliveryStatus: string;
  items: OrderItem[];
  payment?: any;
  
}