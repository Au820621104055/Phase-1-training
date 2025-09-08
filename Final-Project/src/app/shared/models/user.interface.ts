export interface User {
  userId: number;
  fullName: string;
  email: string;
  password: string;
  role: 'Customer' | 'RestaurantOwner' | 'DeliveryStaff' | 'Admin';
  phoneNumber?: string;
  isActive?: boolean;
}