import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../shared/services/auth.service';

@Component({ selector: 'app-login', templateUrl: './login.component.html', styleUrls: ['./login.component.css'] })
export class LoginComponent { email = ''; password = ''; error = ''; constructor(private auth: AuthService, private router: Router) {} onSubmit() { this.error = ''; this.auth.login(this.email, this.password).subscribe({ next: res => { const user = res.user; if (user?.role === 'Admin') this.router.navigate(['/admin/dashboard']); else if (user?.role === 'Restaurant') this.router.navigate(['/restaurant/dashboard']); else if (user?.role === 'Delivery') this.router.navigate(['/delivery/dashboard']); else this.router.navigate(['/customer/dashboard']); }, error: err => this.error = err?.error?.message || 'Login failed' }); } }
