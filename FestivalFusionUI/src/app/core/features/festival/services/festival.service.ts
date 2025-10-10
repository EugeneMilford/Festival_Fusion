import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AddFestivalRequest } from '../models/add-festival-request';
import { Festival } from '../models/festival.model';
import { environment } from '../../../../../environments/environment';
import { UpdateFestivalRequest } from '../models/update-festival-request.model';

@Injectable({
  providedIn: 'root'  // This should be here
})
export class FestivalService {
  constructor(private http: HttpClient) { }

  // Return all festivals
  getAllFestivals(): Observable<Festival[]> {
    return this.http.get<Festival[]>(`${environment.apiBaseUrl}/api/festival`);
  }

  // Add a new festival 
  addFestival(model: AddFestivalRequest): Observable<Festival> {
    return this.http.post<Festival>(`${environment.apiBaseUrl}/api/festival`, model);
  }

  // Get a festival by id
  getFestivalById(id: string): Observable<Festival> {
    return this.http.get<Festival>(`${environment.apiBaseUrl}/api/festival/${id}`);
  }

  // Update a Festival
  updateFestival(id: string, updateFestivalRequest: UpdateFestivalRequest): Observable<Festival> {
    return this.http.put<Festival>(`${environment.apiBaseUrl}/api/festival/${id}`, updateFestivalRequest);
  }

  // Delete a Festival
  deleteFestival(id: string): Observable<Festival> {
    return this.http.delete<Festival>(`${environment.apiBaseUrl}/api/festival/${id}`)
  }
}

