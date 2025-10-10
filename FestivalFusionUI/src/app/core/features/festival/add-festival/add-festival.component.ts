import { Component, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { FestivalService } from '../services/festival.service';
import { FormsModule } from '@angular/forms';
import { AddFestivalRequest } from '../models/add-festival-request';

@Component({
  selector: 'app-add-festival',
  imports: [FormsModule],
  templateUrl: './add-festival.component.html',
  styleUrl: './add-festival.component.css'
})
export class AddFestivalComponent implements OnDestroy {

  model: AddFestivalRequest;

  //Adding subscription
  private addFestivalSubscribtion?: Subscription;

  constructor(private festivalService: FestivalService,
    private router: Router){
    this.model = {
      festivalName: '',
      festivalDescription: '',
      theme: '',
      startDate: '',
      endDate: '',
      sponsor: ''
    }
   }

  //Adding the service to the on submit
  onFormSubmit() {
    this.addFestivalSubscribtion = this.festivalService.addFestival(this.model)
    .subscribe({
      next: (response) => {
        this.router.navigateByUrl('/admin/festival');
      }
    })
  }

  ngOnDestroy(): void {
    this.addFestivalSubscribtion?.unsubscribe();
  }
}


