import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../shared/services/auth.service';

@Component({ selector: 'app-register', templateUrl: './register.component.html', styleUrls: ['./register.component.css'] })
export class RegisterComponent { name = ''; email = ''; password = ''; role: 'Customer' | 'Restaurant' = 'Customer'; error = ''; success = ''; constructor(private auth: AuthService, private router: Router) {} onSubmit() { this.error = this.success = ''; const payload = { name: this.name, email: this.email, password: this.password, role: this.role }; this.auth.register(payload).subscribe({ next: () => { this.success = 'Registration successful. Please login.'; setTimeout(() => this.router.navigate(['/login']), 1200); }, error: (err) => this.error = err?.error?.message || 'Registration failed' }); } }
