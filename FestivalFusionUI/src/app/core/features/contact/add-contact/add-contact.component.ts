import { Component, OnDestroy } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AddContactRequest } from '../models/add-contact-request.model';
import { Subscription } from 'rxjs';
import { ContactService } from '../services/contact.service';
import { Router, RouterLink } from '@angular/router';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-add-contact',
  imports: [FormsModule, NgIf, RouterLink],
  templateUrl: './add-contact.component.html',
  styleUrls: ['./add-contact.component.css']
})
export class AddContactComponent implements OnDestroy {
  model: AddContactRequest;
  private addContactSubscription?: Subscription;
  showModal: boolean = false; // Flag for modal visibility

  constructor(private contactService: ContactService, private router: Router) {
    this.model = {
      name: '',
      surname: '',
      phoneNumber: 0,
      message: ''
    };
  }

  onContactSubmit() {
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