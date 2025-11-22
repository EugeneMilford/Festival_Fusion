import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { AddContactRequest } from '../models/add-contact-request.model';
import { Contact } from '../models/contact.model';

@Injectable({
  providedIn: 'root'
})
export class ContactService {

  constructor(private http: HttpClient) { }

  // Anyone authenticated can post a contact message 
  addContact(model: AddContactRequest): Observable<void> {
    return this.http.post<void>(`${environment.apiBaseUrl}/api/contact?addAuth=true`, model);
  }

  getAllContacts(): Observable<Contact[]> {
    return this.http.get<Contact[]>(`${environment.apiBaseUrl}/api/contact?addAuth=true`);
  }

  deleteContact(id: string): Observable<Contact> {
    return this.http.delete<Contact>(`${environment.apiBaseUrl}/api/contact/${id}?addAuth=true`);
  }
}