import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { VenueService } from '../services/venue.service';
import { Subscription } from 'rxjs';
import { Venue } from '../models/venue.model';
import { UpdateVenueRequest } from '../models/update-venue-request.model';
import { NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-edit-venue',
  imports: [RouterLink, NgIf, FormsModule],
  templateUrl: './edit-venue.component.html',
  styleUrls: ['./edit-venue.component.css']
})
export class EditVenueComponent implements OnInit, OnDestroy {

  venueid: string | null = null;
  paramsSubscription?: Subscription;
  editVenueSubscription?: Subscription;
  venue?: Venue;

  constructor(
    private route: ActivatedRoute,
    private venueService: VenueService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.paramsSubscription = this.route.paramMap.subscribe({
      next: (params) => {
        this.venueid = params.get('id');

        if (this.venueid) {
          // get the data from the API for this Venue Id
          this.venueService.getVenueById(this.venueid)
            .subscribe({
              next: (response) => {
                this.venue = response;
              },
              error: (err) => {
                console.error('Failed to load venue', err);
              }
            });
        }
      },
      error: (err) => {
        console.error('Failed to read route params', err);
      }
    });
  }

  onEditVenueSubmit(): void {
    if (!this.venue || !this.venueid) {
      return;
    }

    const updateVenueRequest: UpdateVenueRequest = {
      Name: this.venue?.name ?? '',
      VenueImageUrl: this.venue.venueImageUrl ?? '',
      Address: this.venue?.address ?? '',
      Capacity: this.venue?.capacity ?? 0
    };

    // call the service to update the venue
    this.editVenueSubscription = this.venueService.updateVenue(this.venueid, updateVenueRequest)
      .subscribe({
        next: (updated) => {
          console.log('Venue updated successfully:', updated);
          // navigate back to the venues list
          this.router.navigate(['/admin/venue']);
        },
        error: (err) => {
          console.error('Failed to update venue', err);
        }
      });
  }

  onRemoveVenue(): void {
    if (this.venueid) {
      this.venueService.removeVenue(this.venueid)
      .subscribe({
        next: (response) => {
          this.router.navigateByUrl('/admin/venue');
        }
      })
    }
  }

  ngOnDestroy(): void {
    this.paramsSubscription?.unsubscribe();
    this.editVenueSubscription?.unsubscribe();
  }
}
