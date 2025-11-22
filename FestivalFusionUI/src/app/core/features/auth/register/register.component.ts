import { Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../services/auth.service';
import { RegisterRequest } from '../models/register-request.model';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, RouterLink, CommonModule],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {

  model: RegisterRequest;
  availableRoles: string[] = ['Reader', 'Writer', 'Editor', 'Moderator'];
  selectedRoles: { [key: string]: boolean } = {};
  errorMessage: string = '';
  successMessage: string = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) {
    this.model = {
      email: '',
      password: '',
      roles: []
    };

    // Initialize all roles as unchecked
    this.availableRoles.forEach(role => {
      this.selectedRoles[role] = false;
    });
  }

  onFormSubmit(): void {
    this.errorMessage = '';
    this.successMessage = '';

    // Get selected roles
    this.model.roles = Object.keys(this.selectedRoles)
      .filter(role => this.selectedRoles[role]);

    // Validate at least one role is selected
    if (this.model.roles.length === 0) {
      this.errorMessage = 'Please select at least one role.';
      return;
    }

    console.log('📝 Registering user...');
    console.log('📧 Email:', this.model.email);
    console.log('🎭 Roles:', this.model.roles);

    this.authService.register(this.model)
      .subscribe({
        next: (response) => {
          console.log('✅ Registration successful!', response);
          this.successMessage = 'Registration successful! Redirecting to login...';
          
          // Redirect to login after 2 seconds
          setTimeout(() => {
            this.router.navigateByUrl('/login');
          }, 2000);
        },
        error: (error) => {
          console.error('❌ Registration failed:', error);
          
          if (error.error && error.error.errors) {
            // Handle validation errors
            const errors = error.error.errors;
            this.errorMessage = Object.values(errors).flat().join(' ');
          } else if (error.error && typeof error.error === 'string') {
            this.errorMessage = error.error;
          } else {
            this.errorMessage = 'Registration failed. Please try again.';
          }
        }
      });
  }

  toggleRole(role: string): void {
    this.selectedRoles[role] = !this.selectedRoles[role];
  }
}