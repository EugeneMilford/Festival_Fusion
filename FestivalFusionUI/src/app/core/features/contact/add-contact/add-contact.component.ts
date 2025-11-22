import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AddContactRequest } from '../models/add-contact-request.model';
import { Subscription } from 'rxjs';
import { ContactService } from '../services/contact.service';
import { Router, RouterLink } from '@angular/router';
import { NgIf, CommonModule } from '@angular/common';
import { AuthService } from '../../../features/auth/services/auth.service';

@Component({
  selector: 'app-add-contact',
  imports: [FormsModule, NgIf, RouterLink, CommonModule],
  templateUrl: './add-contact.component.html',
  styleUrls: ['./add-contact.component.css']
})
export class AddContactComponent implements OnInit, OnDestroy {
  model: AddContactRequest;
  private addContactSubscription?: Subscription;
  showModal: boolean = false; // Flag for modal visibility

  constructor(
    private contactService: ContactService,
    private router: Router,
    private authService: AuthService
  ) {
    this.model = {
      name: '',
      surname: '',
      phoneNumber: 0,
      message: ''
    };
  }

  ngOnInit(): void {
    // If a user is authenticated, optionally prefill contact fields if available
    const user = this.authService.getUser();
    if (user) {
      // Try to prefill common fields if they exist on the user object
      if ((user as any).firstName) {
        this.model.name = (user as any).firstName;
      }
      if ((user as any).lastName) {
        this.model.surname = (user as any).lastName;
      }
      if ((user as any).phoneNumber) {
        // ensure it's a number if stored as string
        const pn = (user as any).phoneNumber;
        this.model.phoneNumber = typeof pn === 'string' ? Number(pn.replace(/\D/g, '')) || 0 : pn || 0;
      }
    }
  }

  // Allow any authenticated user to send a contact message
  isAuthenticated(): boolean {
    return !!this.authService.getUser();
  }

  onContactSubmit() {
    if (!this.isAuthenticated()) {
      // Shouldn't happen in normal flow because UI hides form, but guard anyway.
      console.warn('Attempt to submit contact while unauthenticated');
      return;
    }

    this.addContactSubscription = this.contactService.addContact(this.model)
      .subscribe({
        next: () => {
          this.showModal = true; // Show the modal
        },
        error: (err) => {
          console.error('Failed to add contact', err);
        }
      });
  }

  closeModal() {
    this.showModal = false;
    this.router.navigateByUrl('/admin/contact');
  }

  ngOnDestroy(): void {
    this.addContactSubscription?.unsubscribe();
  }
}