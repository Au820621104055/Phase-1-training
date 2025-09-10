import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../shared/services/auth.service';
import { LoginRequest } from '../../shared/models/auth.interface';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  credentials: LoginRequest = { email: '', password: '' };
  error: string = '';

  constructor(private authService: AuthService, private router: Router) {}

  onSubmit(): void {
    console.log('clicked');
    this.authService.login(this.credentials).subscribe({
      next: (response) => {
        this.authService.saveAuthData(response);

        const role = this.authService.getRole();
        console.log('User role:', role);

        if (role === 'Customer') {
          this.router.navigate(['/customer']);
        } else if (role === 'RestaurantOwner') {
          this.router.navigate(['/restaurant/dashboard']);
        } else if (role === 'DeliveryStaff') {
          this.router.navigate(['/delivery/dashboard']);
        } else if (role === 'Admin') {
          this.router.navigate(['/admin']);
        } else {
          this.router.navigate(['/']);
        }
      },
      error: () => {
        this.error = 'Invalid email or password';
      }
    });
  }
}
