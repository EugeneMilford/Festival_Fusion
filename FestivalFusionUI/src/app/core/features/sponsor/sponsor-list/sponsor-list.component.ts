import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Sponsor } from '../models/sponsor.model';
import { Observable } from 'rxjs';
import { SponsorService } from '../services/sponsor.service';
import { AsyncPipe, NgFor, NgIf, CommonModule } from '@angular/common';
import { AuthService } from '../../../features/auth/services/auth.service';

declare var bootstrap: any;

@Component({
  selector: 'app-sponsor-list',
  imports: [RouterLink, NgIf, NgFor, AsyncPipe, CommonModule],
  templateUrl: './sponsor-list.component.html',
  styleUrl: './sponsor-list.component.css'
})
export class SponsorListComponent implements OnInit {

  // Create an observable variable
  sponsors$?: Observable<Sponsor[]>;
  selectedSponsor?: Sponsor;
  userRoles: string[] = [];

  constructor(
    private sponsorService: SponsorService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.sponsors$ = this.sponsorService.getAllSponsors();
    this.loadUserRoles();
  }

  loadUserRoles(): void {
    const user = this.authService.getUser();
    if (user) {
      this.userRoles = user.roles;
      console.log('👤 User roles:', this.userRoles);
    }
  }

  // Check if user can add sponsors (Writer, Editor)
  canAdd(): boolean {
    return this.userRoles.some(role =>
      ['Writer', 'Editor'].includes(role)
    );
  }

  // Check if user can edit sponsors (Editor, Moderator)
  canEdit(): boolean {
    return this.userRoles.some(role =>
      ['Editor', 'Moderator'].includes(role)
    );
  }

  // Check if user can delete sponsors (Editor only)
  canDelete(): boolean {
    return this.userRoles.includes('Editor');
  }

  openSponsorModal(sponsor: Sponsor): void {
    this.selectedSponsor = sponsor;
    const modalElement = document.getElementById('sponsorModal');
    const modal = new bootstrap.Modal(modalElement);
    modal.show();
  }
}