import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ArtistService } from '../services/artist.service';
import { Subscription } from 'rxjs';
import { Artist } from '../models/artist.model';
import { UpdateArtistRequest } from '../models/update-artist-request.model';
import { NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-edit-artist',
  imports: [NgIf, FormsModule],
  templateUrl: './edit-artist.component.html',
  styleUrls: ['./edit-artist.component.css']
})
export class EditArtistComponent implements OnInit, OnDestroy {

  artistid: string | null = null;
  paramsSubscription?: Subscription;
  editArtistSubscription?: Subscription;
  artist?: Artist;

  constructor(
    private route: ActivatedRoute,
    private artistService: ArtistService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.paramsSubscription = this.route.paramMap.subscribe({
      next: (params) => {
        this.artistid = params.get('id');

        if (this.artistid) {
          this.artistService.getArtistById(this.artistid)
            .subscribe({
              next: (response) => {
                this.artist = response;
              },
              error: (err) => {
                console.error('Failed to load artist', err);
              }
            });
        }
      },
      error: (err) => {
        console.error('Failed to read route params', err);
      }
    });
  }

  onEditArtistSubmit(): void {
    if (!this.artist || !this.artistid) {
      return;
    }

    const updateArtistRequest: UpdateArtistRequest = {
      name: this.artist.name ?? '',
      artistImageUrl: this.artist.artistImageUrl ?? '',
      genre: this.artist.genre ?? '',
      country: this.artist.country ?? '',
      bio: this.artist.bio ?? ''
    };

    // Call the service to update the artist
    this.editArtistSubscription = this.artistService.updateArtist(this.artistid, updateArtistRequest)
      .subscribe({
        next: (response) => {
          console.log('Artist updated successfully:', response);
          this.router.navigate(['/admin/artist']); // Navigate back to artists list
        },
        error: (error) => {
          console.error('Error updating artist:', error);
        }
      });
  }

  onRemoveArtist(): void {
    if (this.artistid) {
      this.artistService.removeArtist(this.artistid)
      .subscribe({
        next: (response) => {
          this.router.navigateByUrl('/admin/artist');
        }
      })
    }
  }


  ngOnDestroy(): void {
    this.paramsSubscription?.unsubscribe();
    this.editArtistSubscription?.unsubscribe();
  }
}