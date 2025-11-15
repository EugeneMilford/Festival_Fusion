import { Component, OnDestroy } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AddSponsorRequest } from '../models/add-sponsor-request.model';
import { Subscription } from 'rxjs';
import { SponsorService } from '../services/sponsor.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-sponsor',
  imports: [FormsModule],
  templateUrl: './add-sponsor.component.html',
  styleUrls: ['./add-sponsor.component.css']
})
export class AddSponsorComponent implements OnDestroy {

  model: AddSponsorRequest;

  // Adding subscription
  private addSponsorSubscription?: Subscription;

  constructor(private sponsorService: SponsorService,
              private router: Router) {
    this.model = {
      Name: '',
      ContactEmail: '',
      Phone: '',
      Website: ''
    };
  }

  // Submit handler
  onSponsorSubmit() {
    this.addSponsorSubscription = this.sponsorService.addSponsor(this.model)
      .subscribe({
        next: () => {
          // Navigate back to sponsors list (adjust route if your app uses a different path)
          this.router.navigateByUrl('/admin/sponsor');
        },
        error: (err) => {
          console.error('Failed to add sponsor', err);
        }
      });
  }

  ngOnDestroy(): void {
    this.addSponsorSubscription?.unsubscribe();
  }
}

