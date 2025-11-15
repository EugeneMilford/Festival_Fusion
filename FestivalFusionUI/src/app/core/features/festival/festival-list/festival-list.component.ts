import { Component, OnInit } from '@angular/core';
import { Festival } from '../models/festival.model';
import { FestivalService } from '../services/festival.service';
import { Observable } from 'rxjs';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { AsyncPipe, CommonModule, NgFor, NgIf } from '@angular/common';

declare var bootstrap: any;

@Component({
  selector: 'app-festival-list',
  imports: [RouterLink, NgFor, NgIf, AsyncPipe],
  templateUrl: './festival-list.component.html',
  styleUrl: './festival-list.component.css'
})
export class FestivalListComponent implements OnInit {
  
  // Create an observable variable
  festivals$?: Observable<Festival[]>;
  selectedFestival?: Festival;

  constructor(private festivalService: FestivalService) {}

  ngOnInit(): void {
    this.festivals$ = this.festivalService.getAllFestivals();
  }

  openFestivalModal(festival: Festival): void {
    this.selectedFestival = festival;
    const modalElement = document.getElementById('festivalModal');
    const modal = new bootstrap.Modal(modalElement);
    modal.show();
  }
}


