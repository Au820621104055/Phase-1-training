import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../shared/services/auth.service';
import { RegisterRequest } from '../../shared/models/auth.interface';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls:['./register.component.css']
})
export class RegisterComponent {
  user: RegisterRequest = {
    fullName: '',
    email: '',
    password: '',
    role: 'Customer',
    phoneNumber: ''
  };
  error: string = '';

  constructor(private authService: AuthService, private router: Router) {}

  onSubmit(): void {
    this.authService.register(this.user).subscribe({
      next: (response) => {
        this.authService.saveAuthData(response); 
        this.router.navigate(['/login']);
      },
      error: () => {
        this.error = 'Registration failed. Please try again.';
      }
    });
  }
}
