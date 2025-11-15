import { Component, OnDestroy } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AddArtistRequest } from '../models/add-artist-request.model';
import { Subscription } from 'rxjs';
import { ArtistService } from '../services/artist.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-artist',
  imports: [FormsModule],
  templateUrl: './add-artist.component.html',
  styleUrl: './add-artist.component.css'
})
export class AddArtistComponent implements OnDestroy{

  model: AddArtistRequest;

  //Adding subscription
  private addArtistSubscribtion?: Subscription;

  constructor(private artistService: ArtistService, 
    private router: Router){
    this.model = {
      name: '',
      artistImageUrl: '',
      genre: '',
      country: '',
      bio: ''
    }
  }

  //Adding the service to the on submit
  onArtistSubmit(){
    this.addArtistSubscribtion = this.artistService.addArtist(this.model)
    .subscribe({
      next: (response) => {
        this.router.navigateByUrl('/admin/artist');
      }
    })
  }

  ngOnDestroy(): void {
    this.addArtistSubscribtion?.unsubscribe();
  }
}


