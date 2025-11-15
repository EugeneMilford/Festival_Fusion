import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Observable } from 'rxjs';
import { Artist } from '../models/artist.model';
import { ArtistService } from '../services/artist.service';
import { AsyncPipe, NgFor, NgIf } from '@angular/common';

declare var bootstrap: any;

@Component({
  selector: 'app-artist-list',
  imports: [RouterLink, NgIf, NgFor, AsyncPipe],
  templateUrl: './artist-list.component.html',
  styleUrl: './artist-list.component.css'
})
export class ArtistListComponent implements OnInit {
  
  // Create an observable variable
  artists$?: Observable<Artist[]>;
  selectedArtist?: Artist;

  constructor(private artistService: ArtistService) {}

  ngOnInit(): void {
    this.artists$ = this.artistService.getAllArtists();
  }

  openArtistModal(artist: Artist): void {
    this.selectedArtist = artist;
    const modalElement = document.getElementById('artistModal');
    const modal = new bootstrap.Modal(modalElement);
    modal.show();
  }
}

