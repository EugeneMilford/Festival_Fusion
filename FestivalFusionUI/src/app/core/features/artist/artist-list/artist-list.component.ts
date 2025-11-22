import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Observable } from 'rxjs';
import { Artist } from '../models/artist.model';
import { ArtistService } from '../services/artist.service';
import { AuthService } from '../../../features/auth/services/auth.service';
import { AsyncPipe, NgFor, NgIf, CommonModule } from '@angular/common';

declare var bootstrap: any;

@Component({
  selector: 'app-artist-list',
  imports: [RouterLink, NgIf, NgFor, AsyncPipe, CommonModule],
  templateUrl: './artist-list.component.html',
  styleUrl: './artist-list.component.css'
})
export class ArtistListComponent implements OnInit {
  
  // Create an observable variable
  artists$?: Observable<Artist[]>;
  selectedArtist?: Artist;
  userRoles: string[] = [];

  constructor(
    private artistService: ArtistService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.artists$ = this.artistService.getAllArtists();
    this.loadUserRoles();
  }

  loadUserRoles(): void {
    const user = this.authService.getUser();
    if (user) {
      this.userRoles = user.roles;
      console.log('👤 User roles:', this.userRoles);
    }
  }

  // Check if user can add artists (Writer, Editor)
  canAdd(): boolean {
    return this.userRoles.some(role => 
      ['Writer', 'Editor'].includes(role)
    );
  }

  // Check if user can edit artists (Editor, Moderator)
  canEdit(): boolean {
    return this.userRoles.some(role => 
      ['Editor', 'Moderator'].includes(role)
    );
  }

  // Check if user can delete artists (Editor only)
  canDelete(): boolean {
    return this.userRoles.includes('Editor');
  }

  openArtistModal(artist: Artist): void {
    this.selectedArtist = artist;
    const modalElement = document.getElementById('artistModal');
    const modal = new bootstrap.Modal(modalElement);
    modal.show();
  }
}