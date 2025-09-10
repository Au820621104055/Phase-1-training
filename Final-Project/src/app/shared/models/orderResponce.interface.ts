import { OrderDetail } from './orderDetails.interface';
import { OrderItem } from "./orderitem.interface";

export interface orderReponce{
    total:number;
    date:Date;
    status:string;
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
    orderdetail:OrderDetail[];
    payment?: any;
    totalAmount:number;
}