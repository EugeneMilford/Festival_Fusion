import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { VenueService } from '../services/venue.service';
import { Venue } from '../models/venue.model';
import { Observable } from 'rxjs';
import { AsyncPipe, NgFor, NgIf } from '@angular/common';

declare var bootstrap: any;

@Component({
  selector: 'app-venue-list',
  imports: [RouterLink, NgFor, NgIf, AsyncPipe],
  templateUrl: './venue-list.component.html',
  styleUrl: './venue-list.component.css'
})
export class VenueListComponent implements OnInit {

  // Create an observable variable
  venues$?: Observable<Venue[]>;
  selectedVenue?: Venue;

  constructor(private venueService: VenueService) {}

  ngOnInit(): void {
    this.venues$ = this.venueService.getAllVenues();
  }

  openVenueModal(venue: Venue): void {
    this.selectedVenue = venue;
    const modalElement = document.getElementById('venueModal');
    const modal = new bootstrap.Modal(modalElement);
    modal.show();
  }
}
