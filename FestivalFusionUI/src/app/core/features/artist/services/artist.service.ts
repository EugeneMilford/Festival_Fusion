import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AddArtistRequest } from '../models/add-artist-request.model';
import { Artist } from '../models/artist.model';
import { environment } from '../../../../../environments/environment';
import { UpdateArtistRequest } from '../models/update-artist-request.model';

@Injectable({
  providedIn: 'root'
})
export class ArtistService {

  constructor(private http: HttpClient) { }

  //Takes request from model + passes to API
  addArtist(model: AddArtistRequest): Observable<void> {
    return this.http.post<void>(`${environment.apiBaseUrl}/api/artist?addAuth=true`, model);
  }

  //return all artists
  getAllArtists(): Observable<Artist[]>{
    return this.http.get<Artist[]>(`${environment.apiBaseUrl}/api/artist?addAuth=true`);
  }

  // Get an Artist by id
  getArtistById(id: string): Observable<Artist> {
    return this.http.get<Artist>(`${environment.apiBaseUrl}/api/artist/${id}?addAuth=true`);
  }

  // Update an Artist
  updateArtist(id: string, updateArtistRequest: UpdateArtistRequest): Observable<Artist> {
    return this.http.put<Artist>(`${environment.apiBaseUrl}/api/artist/${id}?addAuth=true`, updateArtistRequest);
  }

  // Delete an Artist
  removeArtist(id: string): Observable<Artist> {
    return this.http.delete<Artist>(`${environment.apiBaseUrl}/api/artist/${id}?addAuth=true`);
  }
}
