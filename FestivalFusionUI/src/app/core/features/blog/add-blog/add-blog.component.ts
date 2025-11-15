import { Component, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { BlogService } from '../services/blog.service';
import { AddBlogRequest } from '../models/add-blog-request.model';

@Component({
  selector: 'app-add-blog',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './add-blog.component.html',
  styleUrl: './add-blog.component.css'
})
export class AddBlogComponent implements OnDestroy {

  model: AddBlogRequest;

  // Adding subscription
  private addBlogSubscription?: Subscription;

  constructor(
    private blogService: BlogService,
    private router: Router
  ) {
    this.model = {
      title: '',
      content: '',
      category: '',
      featuredImageUrl: '',
      author: '',
      publishedDate: null,
      updatedDate: null,
      isPublished: false,
      isFeatured: false
    };
  }

  // Adding the service to the on submit
  onFormSubmit() {
    
    // Set updatedDate to current date when creating
    this.model.updatedDate = new Date().toISOString();
    
    // If publishedDate is not set but isPublished is true, set it to now
    if (this.model.isPublished && !this.model.publishedDate) {
      this.model.publishedDate = new Date().toISOString();
    }

    this.addBlogSubscription = this.blogService.addBlogPost(this.model)
      .subscribe({
        next: (response) => {
          console.log('Blog post created successfully:', response);
          this.router.navigateByUrl('/admin/blog');
        },
        error: (error) => {
          console.error('Error creating blog post:', error);
        }
      });
  }

  ngOnDestroy(): void {
    this.addBlogSubscription?.unsubscribe();
  }
}
