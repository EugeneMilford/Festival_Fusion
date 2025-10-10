import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router} from '@angular/router';
import { Subscription } from 'rxjs';
import { Festival } from '../models/festival.model';
import { FestivalService } from '../services/festival.service';
import { FormsModule } from '@angular/forms';
import { NgIf } from '@angular/common';
import { UpdateFestivalRequest } from '../models/update-festival-request.model';

@Component({
  selector: 'app-edit-festival',
  imports: [FormsModule,NgIf],
  templateUrl: './edit-festival.component.html',
  styleUrl: './edit-festival.component.css'
})
export class EditFestivalComponent implements OnInit, OnDestroy{

  festivalid: string | null = null;
  paramsSubscription?: Subscription;
  editFestivalSubscription?: Subscription;
  festival?: Festival; // create local variable

  constructor(private route: ActivatedRoute, 
    private festivalService: FestivalService,
    private router: Router) {

  }

  ngOnInit(): void {
    this.paramsSubscription = this.route.paramMap.subscribe({
      next: (params) => {
        this.festivalid = params.get('id');

        if (this.festivalid) {
          // get the data from the API for this category Id
          this.festivalService.getFestivalById(this.festivalid)
          .subscribe({
            next: (response) => {
              this.festival = response; // use local variable
            }
          });
        }
      }
    });
  }

  onFormSubmit(): void {
    const updateFestivalRequest: UpdateFestivalRequest = {
      festivalName: this.festival?.festivalName ?? '',
      festivalDescription: this.festival?.festivalDescription ?? '',
      theme: this.festival?.theme ?? '',
      startDate: this.festival?.startDate ?? '',
      endDate: this.festival?.endDate ?? '',
      sponsor: this.festival?.sponsor ?? ''
    };

    // pass object to service
    if (this.festivalid) {
      this.editFestivalSubscription = this.festivalService.updateFestival(this.festivalid, updateFestivalRequest)
      .subscribe({
        next: (response) => {
          this.router.navigateByUrl('/admin/festival');
        }
      });
    }
  }

  onDelete(): void {
    if (this.festivalid) {
      this.festivalService.deleteFestival(this.festivalid)
      .subscribe({
        next: (response) => {
          this.router.navigateByUrl('/admin/festival');
        }
      })
    }
  }

  ngOnDestroy(): void {
    this.paramsSubscription?.unsubscribe();
  }
}


