import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Sponsor } from '../models/sponsor.model';
import { Observable } from 'rxjs';
import { SponsorService } from '../services/sponsor.service';
import { AsyncPipe, NgFor, NgIf } from '@angular/common';

declare var bootstrap: any;

@Component({
  selector: 'app-sponsor-list',
  imports: [RouterLink, NgIf, NgFor, AsyncPipe],
  templateUrl: './sponsor-list.component.html',
  styleUrl: './sponsor-list.component.css'
})
export class SponsorListComponent implements OnInit {

  // Create an observable variable
  sponsors$?: Observable<Sponsor[]>;
  selectedSponsor?: Sponsor;

  constructor(private sponsorService: SponsorService) {}

  ngOnInit(): void {
    this.sponsors$ = this.sponsorService.getAllSponsors();
  }

  openSponsorModal(sponsor: Sponsor): void {
    this.selectedSponsor = sponsor;
    const modalElement = document.getElementById('sponsorModal');
    const modal = new bootstrap.Modal(modalElement);
    modal.show();
  }
}