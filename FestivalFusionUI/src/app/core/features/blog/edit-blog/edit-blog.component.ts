import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Subscription } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { CommonModule, NgIf } from '@angular/common';
import { BlogService } from '../services/blog.service';
import { Blog } from '../models/blog.model';
import { UpdateBlogRequest } from '../models/update-blog-request.model';

@Component({
  selector: 'app-edit-blog',
  standalone: true,
  imports: [NgIf, FormsModule, CommonModule, RouterLink],
  templateUrl: './edit-blog.component.html',
  styleUrl: './edit-blog.component.css'
})
export class EditBlogComponent implements OnInit, OnDestroy {

  blogId: string | null = null;
  paramsSubscription?: Subscription;
  editBlogSubscription?: Subscription;
  deleteBlogSubscription?: Subscription;
  blog?: Blog;

  constructor(
    private route: ActivatedRoute,
    private blogService: BlogService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.paramsSubscription = this.route.paramMap.subscribe({
      next: (params) => {
        this.blogId = params.get('id');

        if (this.blogId) {
          this.blogService.getBlogById(this.blogId)
            .subscribe({
              next: (response) => {
                this.blog = response;
              },
              error: (err) => {
                console.error('Failed to load blog post', err);
              }
            });
        }
      },
      error: (err) => {
        console.error('Failed to read route params', err);
      }
    });
  }

  onEditBlogSubmit(): void {
    if (!this.blog || !this.blogId) {
      return;
    }

    // Set updatedDate to current date
    const currentDate = new Date().toISOString();

    const updateBlogRequest: UpdateBlogRequest = {
      title: this.blog.title ?? '',
      content: this.blog.content ?? '',
      category: this.blog.category ?? '',
      featuredImageUrl: this.blog.featuredImageUrl ?? '',
      author: this.blog.author ?? '',
      publishedDate: this.blog.publishedDate ?? null,
      updatedDate: currentDate,
      isPublished: this.blog.isPublished ?? false,
      isFeatured: this.blog.isFeatured ?? false
    };

    // Call the service to update the blog post
    this.editBlogSubscription = this.blogService.updateBlogPost(this.blogId, updateBlogRequest)
      .subscribe({
        next: (response) => {
          console.log('Blog post updated successfully:', response);
          this.router.navigate(['/admin/blog']); // Navigate back to blog list
        },
        error: (error) => {
          console.error('Error updating blog post:', error);
        }
      });
  }

  onRemoveBlog(): void {
    if (this.blogId) {
      // Show confirmation dialog
      if (confirm('Are you sure you want to delete this blog post? This action cannot be undone.')) {
        this.deleteBlogSubscription = this.blogService.deleteBlogPost(this.blogId)
          .subscribe({
            next: (response) => {
              console.log('Blog post deleted successfully:', response);
              this.router.navigateByUrl('/admin/blog');
            },
            error: (error) => {
              console.error('Error deleting blog post:', error);
            }
          });
      }
    }
  }

  ngOnDestroy(): void {
    this.paramsSubscription?.unsubscribe();
    this.editBlogSubscription?.unsubscribe();
    this.deleteBlogSubscription?.unsubscribe();
  }
}