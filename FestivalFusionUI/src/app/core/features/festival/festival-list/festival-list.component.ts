import { Component, OnInit } from '@angular/core';
import { Festival } from '../models/festival.model';
import { FestivalService } from '../services/festival.service';
import { AuthService } from '../../../features/auth/services/auth.service';
import { Observable } from 'rxjs';
import { RouterLink } from '@angular/router';
import { AsyncPipe, CommonModule, NgFor, NgIf } from '@angular/common';

declare var bootstrap: any;

@Component({
  selector: 'app-festival-list',
  imports: [RouterLink, NgFor, NgIf, AsyncPipe, CommonModule],
  templateUrl: './festival-list.component.html',
  styleUrl: './festival-list.component.css'
})
export class FestivalListComponent implements OnInit {
  
  festivals$?: Observable<Festival[]>;
  selectedFestival?: Festival;
  userRoles: string[] = [];

  constructor(
    private festivalService: FestivalService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.festivals$ = this.festivalService.getAllFestivals();
    this.loadUserRoles();
  }

  loadUserRoles(): void {
    const user = this.authService.getUser();
    if (user) {
      this.userRoles = user.roles;
      console.log('👤 User roles:', this.userRoles);
    }
  }

  // Check if user can add festivals (Writer, Editor)
  canAdd(): boolean {
    return this.userRoles.some(role => 
      ['Writer', 'Editor'].includes(role)
    );
  }

  // Check if user can edit festivals (Writer, Editor, Moderator)
  canEdit(): boolean {
    return this.userRoles.some(role => 
      ['Editor', 'Moderator'].includes(role)
    );
  }

  // Check if user can delete festivals (Editor only)
  canDelete(): boolean {
    return this.userRoles.includes('Editor');
  }

  openFestivalModal(festival: Festival): void {
    this.selectedFestival = festival;
    const modalElement = document.getElementById('festivalModal');
    const modal = new bootstrap.Modal(modalElement);
    modal.show();
  }
}

