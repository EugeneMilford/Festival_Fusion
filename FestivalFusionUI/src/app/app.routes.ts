import { Routes } from '@angular/router';
import { FestivalListComponent } from './core/features/festival/festival-list/festival-list.component';
import { HomeComponent } from './core/components/home/home.component';
import { AddFestivalComponent } from './core/features/festival/add-festival/add-festival.component';
import { EditFestivalComponent } from './core/features/festival/edit-festival/edit-festival.component';
import { ArtistListComponent } from './core/features/artist/artist-list/artist-list.component';
import { AddArtistComponent } from './core/features/artist/add-artist/add-artist.component';
import { VenueListComponent } from './core/features/venue/venue-list/venue-list.component';
import { AddVenueComponent } from './core/features/venue/add-venue/add-venue.component';
import { SponsorListComponent } from './core/features/sponsor/sponsor-list/sponsor-list.component';
import { AddSponsorComponent } from './core/features/sponsor/add-sponsor/add-sponsor.component';
import { AddContactComponent } from './core/features/contact/add-contact/add-contact.component';
import { EditArtistComponent } from './core/features/artist/edit-artist/edit-artist.component';
import { EditSponsorComponent } from './core/features/sponsor/edit-sponsor/edit-sponsor.component';
import { EditVenueComponent } from './core/features/venue/edit-venue/edit-venue.component';
import { BlogListComponent } from './core/features/blog/blog-list/blog-list.component';
import { AddBlogComponent } from './core/features/blog/add-blog/add-blog.component';
import { EditBlogComponent } from './core/features/blog/edit-blog/edit-blog.component';

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
    },
    {
        path: 'admin/artist',
        component: ArtistListComponent
    },
    {
        path: 'admin/artist/add',
        component: AddArtistComponent
    },  
    {
        path: 'admin/artist/:id',
        component: EditArtistComponent
    },
    {
        path: 'admin/venue',
        component: VenueListComponent
    },
    {
        path: 'admin/venue/add',
        component: AddVenueComponent
    },
    {
        path: 'admin/venue/:id',
        component: EditVenueComponent
    },
    {
        path: 'admin/sponsor',
        component: SponsorListComponent
    },
    {
        path: 'admin/sponsor/add',
        component: AddSponsorComponent
    },
    {
        path: 'admin/sponsor/:id',
        component: EditSponsorComponent
    },
    {
        path: 'admin/contact',
        component: AddContactComponent
    }, 
    {
        path: 'admin/blog',
        component: BlogListComponent
    },
    {
        path: 'admin/blog/add',
        component: AddBlogComponent
    },
    {
        path: 'admin/blog/:id',
        component: EditBlogComponent
    },
];
