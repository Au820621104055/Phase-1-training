import { Order } from './order.interface';

export interface Payment {
  paymentId: number;
  orderId: number;
  order: Order;
  amount: number;
  paymentMethod: string;   
  paymentStatus: string;   
  paymentDate: string;     
}