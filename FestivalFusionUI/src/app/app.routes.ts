import { Routes } from '@angular/router';
import { FestivalListComponent } from './core/features/festival/festival-list/festival-list.component';
import { HomeComponent } from './core/components/home/home.component';
import { AddFestivalComponent } from './core/features/festival/add-festival/add-festival.component';
import { EditFestivalComponent } from './core/features/festival/edit-festival/edit-festival.component';

export const routes: Routes = [
    {
        path: 'home',
        component: HomeComponent
    },
    {
        path: 'admin/festival',
        component: FestivalListComponent
    },
    {
        path: 'admin/festival/add',
        component: AddFestivalComponent
    },
    {
        path: 'admin/festival/:id',
        component: EditFestivalComponent
    }
];
