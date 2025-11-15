import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { SponsorService } from '../services/sponsor.service';
import { Subscription } from 'rxjs';
import { Sponsor } from '../models/sponsor.model';
import { UpdateSponsorRequest } from '../models/update-sponsor-request.model';
import { FormsModule } from '@angular/forms';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-edit-sponsor',
  imports: [NgIf, FormsModule, RouterLink],
  templateUrl: './edit-sponsor.component.html',
  styleUrls: ['./edit-sponsor.component.css']
})
export class EditSponsorComponent implements OnInit, OnDestroy {

  sponsorid: string | null = null;
  paramsSubscription?: Subscription;
  editSponsorSubscription?: Subscription;
  sponsor?: Sponsor;

  constructor(
    private route: ActivatedRoute,
    private sponsorService: SponsorService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.paramsSubscription = this.route.paramMap.subscribe({
      next: (params) => {
        this.sponsorid = params.get('id');

        if (this.sponsorid) {
          // get the data from the API for this Sponsor Id
          this.sponsorService.getSponsorById(this.sponsorid)
            .subscribe({
              next: (response) => {
                this.sponsor = response;
              },
              error: (err) => {
                console.error('Failed to load sponsor', err);
              }
            });
        }
      },
      error: (err) => {
        console.error('Failed to read route params', err);
      }
    });
  }

  onEditSponsorSubmit(): void {
    if (!this.sponsorid) {
      console.warn('No sponsor id available for update');
      return;
    }

    const updateSponsorRequest: UpdateSponsorRequest = {
      Name: this.sponsor?.name ?? '',
      ContactEmail: this.sponsor?.contactEmail ?? '',
      Phone: this.sponsor?.phone ?? '',
      Website: this.sponsor?.website ?? ''
    };

    // call the service to update the sponsor
    this.editSponsorSubscription = this.sponsorService.updateSponsor(this.sponsorid, updateSponsorRequest)
      .subscribe({
        next: (updated) => {
          // navigate back to the sponsors list or to the updated sponsor's page
          this.router.navigate(['/admin/sponsor']);
        },
        error: (err) => {
          console.error('Failed to update sponsor', err);
        }
      });
  }

  onRemoveSponsor(): void {
    if (this.sponsorid) {
      this.sponsorService.removeSponsor(this.sponsorid)
      .subscribe({
        next: (response) => {
          this.router.navigateByUrl('/admin/artist');
        }
      })
    }
  }

  ngOnDestroy(): void {
    this.paramsSubscription?.unsubscribe();
    this.editSponsorSubscription?.unsubscribe();
  }
}
