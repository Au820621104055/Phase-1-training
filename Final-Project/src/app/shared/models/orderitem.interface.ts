import { Order } from './order.interface';
import { MenuItem } from './menuitem.interface';

export interface OrderItem {
  orderItemId: number;
  orderId: number;
  menuItemId: number;
  price: number;
  quantity: number;
  name?: string;
}