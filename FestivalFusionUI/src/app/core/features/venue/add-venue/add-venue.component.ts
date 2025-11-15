import { Component, OnDestroy } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AddVenueRequest } from '../models/add-venue-request.model';
import { Subscription } from 'rxjs';
import { VenueService } from '../services/venue.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-venue',
  imports: [FormsModule],
  templateUrl: './add-venue.component.html',
  styleUrls: ['./add-venue.component.css']
})
export class AddVenueComponent implements OnDestroy {

  model: AddVenueRequest;

  // Adding subscription
  private addVenueSubscription?: Subscription;

  constructor(private venueService: VenueService,
              private router: Router) {
    this.model = {
      Name: '',
      VenueImageUrl: '',
      Address: '',
      Capacity: 0
    };
  }

  // Submit handler
  onVenueSubmit() {
    this.addVenueSubscription = this.venueService.addVenue(this.model)
      .subscribe({
        next: () => {
          // Navigate back to venues list (adjust route if your app uses /admin/venues)
          this.router.navigateByUrl('/admin/venue');
        },
        error: (err) => {
          console.error('Failed to add venue', err);
        }
      });
  }

  ngOnDestroy(): void {
    this.addVenueSubscription?.unsubscribe();
  }
}



