import { Component } from '@angular/core';
import { LoginRequest } from '../models/login-request.model';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-login',
  imports: [FormsModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  model: LoginRequest;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {
    this.model = {
      email: '',
      password: ''
    };
  }

  onFormSubmit(): void {
    console.log('🔐 Logging in...');
    
    this.authService.login(this.model)
    .subscribe({
      next: (response) => {
        console.log('✅ Login successful!');
        console.log('🔑 Token:', response.token);
        
        // Store token in localStorage (NOT cookie)
        localStorage.setItem('auth-token', response.token);
        console.log('💾 Token saved to localStorage');

        // Set User
        this.authService.setUser({
          email: response.email,
          roles: response.roles
        });

        // Redirect back to Home
        this.router.navigateByUrl('/home');
      },
      error: (error) => {
        console.error('❌ Login failed:', error);
      }
    });
  }
}


