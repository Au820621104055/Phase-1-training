export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  fullName: string;
  email: string;
  password: string;
  role: string;       
  phoneNumber?: string;
}

export interface AuthResponse {
  token: string;
}
