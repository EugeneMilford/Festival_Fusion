import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AddSponsorRequest } from '../models/add-sponsor-request.model';
import { environment } from '../../../../../environments/environment';
import { Sponsor } from '../models/sponsor.model';
import { UpdateSponsorRequest } from '../models/update-sponsor-request.model';

@Injectable({
  providedIn: 'root'
})
export class SponsorService {

  constructor(private http: HttpClient) { }

  // Adding a Sponsor
  addSponsor(model: AddSponsorRequest): Observable<void> {
    return this.http.post<void>(`${environment.apiBaseUrl}/api/sponsor?addAuth=true`, model);
  }

  // Return all Sponsors
  getAllSponsors(): Observable<Sponsor[]>{
    return this.http.get<Sponsor[]>(`${environment.apiBaseUrl}/api/sponsor?addAuth=true`);
  }

  // Get a Sponsor by id
  getSponsorById(id: string): Observable<Sponsor> {
    return this.http.get<Sponsor>(`${environment.apiBaseUrl}/api/sponsor/${id}?addAuth=true`);
  }

  // Update a Sponsor
  updateSponsor(id: string, updateSponsorRequest: UpdateSponsorRequest): Observable<Sponsor> {
    return this.http.put<Sponsor>(`${environment.apiBaseUrl}/api/sponsor/${id}?addAuth=true`, updateSponsorRequest);
  }

  // Delete a Sponsor
  removeSponsor(id: string): Observable<Sponsor> {
    return this.http.delete<Sponsor>(`${environment.apiBaseUrl}/api/sponsor/${id}?addAuth=true`)
  }
}