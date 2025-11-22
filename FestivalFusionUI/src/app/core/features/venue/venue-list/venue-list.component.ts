import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { VenueService } from '../services/venue.service';
import { Venue } from '../models/venue.model';
import { Observable } from 'rxjs';
import { AsyncPipe, NgFor, NgIf, CommonModule } from '@angular/common';
import { AuthService } from '../../../features/auth/services/auth.service';

declare var bootstrap: any;

@Component({
  selector: 'app-venue-list',
  imports: [RouterLink, NgFor, NgIf, AsyncPipe, CommonModule],
  templateUrl: './venue-list.component.html',
  styleUrl: './venue-list.component.css'
})
export class VenueListComponent implements OnInit {

  // Create an observable variable
  venues$?: Observable<Venue[]>;
  selectedVenue?: Venue;
  userRoles: string[] = [];

  constructor(
    private venueService: VenueService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.venues$ = this.venueService.getAllVenues();
    this.loadUserRoles();
  }

  loadUserRoles(): void {
    const user = this.authService.getUser();
    if (user) {
      this.userRoles = user.roles;
      console.log('👤 User roles:', this.userRoles);
    }
  }

  // Check if user can add venues (Writer, Editor)
  canAdd(): boolean {
    return this.userRoles.some(role =>
      ['Writer', 'Editor'].includes(role)
    );
  }

  // Check if user can edit venues (Editor, Moderator)
  canEdit(): boolean {
    return this.userRoles.some(role =>
      ['Editor', 'Moderator'].includes(role)
    );
  }

  // Check if user can delete venues (Editor only)
  canDelete(): boolean {
    return this.userRoles.includes('Editor');
  }

  openVenueModal(venue: Venue): void {
    this.selectedVenue = venue;
    const modalElement = document.getElementById('venueModal');
    const modal = new bootstrap.Modal(modalElement);
    modal.show();
  }
}