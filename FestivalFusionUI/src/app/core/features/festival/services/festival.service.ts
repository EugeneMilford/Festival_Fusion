import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AddFestivalRequest } from '../models/add-festival-request';
import { Festival } from '../models/festival.model';
import { environment } from '../../../../../environments/environment';
import { UpdateFestivalRequest } from '../models/update-festival-request.model';

@Injectable({
  providedIn: 'root'
})
export class FestivalService {
  
  constructor(private http: HttpClient) { }  // Removed CookieService

  // Return all festivals
  getAllFestivals(): Observable<Festival[]> {
    return this.http.get<Festival[]>(`${environment.apiBaseUrl}/api/festival?addAuth=true`);
  }

  // Add a new festival
  addFestival(model: AddFestivalRequest): Observable<Festival> {
    return this.http.post<Festival>(`${environment.apiBaseUrl}/api/festival?addAuth=true`, model);
  }

  // Get a festival by id
  getFestivalById(id: string): Observable<Festival> {
    return this.http.get<Festival>(`${environment.apiBaseUrl}/api/festival/${id}?addAuth=true`);
  }

  // Update a Festival
  updateFestival(id: string, updateFestivalRequest: UpdateFestivalRequest): Observable<Festival> {
    return this.http.put<Festival>(`${environment.apiBaseUrl}/api/festival/${id}?addAuth=true`, updateFestivalRequest);
  }

  // Delete a Festival
  deleteFestival(id: string): Observable<Festival> {
    return this.http.delete<Festival>(`${environment.apiBaseUrl}/api/festival/${id}?addAuth=true`);
  }
}


