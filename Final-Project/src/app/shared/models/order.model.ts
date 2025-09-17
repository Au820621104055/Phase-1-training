export interface OrderItemDto { menuItemId: number; quantity: number; notes?: string; }
export interface OrderDto { id?: number; customerId?: number; restaurantId: number; items: OrderItemDto[]; deliveryAddress: string; paymentMethod: 'COD' | 'CARD' | 'ONLINE'; totalAmount?: number; status?: string; createdAt?: string; }
