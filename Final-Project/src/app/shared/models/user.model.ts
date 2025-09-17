export interface User { id?: number; name?: string; email: string; role?: 'Customer' | 'Restaurant' | 'Delivery' | 'Admin'; status?: string; token?: string; }
