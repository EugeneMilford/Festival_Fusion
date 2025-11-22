import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AddVenueRequest } from '../models/add-venue-request.model';
import { Venue } from '../models/venue.model';
import { environment } from '../../../../../environments/environment';
import { UpdateVenueRequest } from '../models/update-venue-request.model';

@Injectable({
  providedIn: 'root'
})
export class VenueService {

  constructor(private http: HttpClient) { }

  // Adding a venue
  addVenue(model: AddVenueRequest): Observable<void> {
    return this.http.post<void>(`${environment.apiBaseUrl}/api/venue?addAuth=true`, model);
  }

  // Return all venues
  getAllVenues(): Observable<Venue[]>{
    return this.http.get<Venue[]>(`${environment.apiBaseUrl}/api/venue?addAuth=true`);
  }

  // Get a Venue by id
  getVenueById(id: string): Observable<Venue> {
    return this.http.get<Venue>(`${environment.apiBaseUrl}/api/venue/${id}?addAuth=true`);
  }

  // Update a Venue
  updateVenue(id: string, updateVenueRequest: UpdateVenueRequest): Observable<Venue> {
    return this.http.put<Venue>(`${environment.apiBaseUrl}/api/venue/${id}?addAuth=true`, updateVenueRequest);
  }

  // Remove a Venue
  removeVenue(id: string): Observable<Venue> {
    return this.http.delete<Venue>(`${environment.apiBaseUrl}/api/venue/${id}?addAuth=true`)
  }
}