import { Routes } from '@angular/router';
import { FestivalListComponent } from './core/features/festival/festival-list/festival-list.component';
import { HomeComponent } from './core/components/home/home.component';

export const routes: Routes = [
    {
        path: 'home',
        component: HomeComponent
    },
    {
        path: 'admin/festival',
        component: FestivalListComponent
    }
];
