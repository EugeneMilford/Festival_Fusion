import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { BlogService } from '../services/blog.service';
import { Blog } from '../models/blog.model';

declare var bootstrap: any; // For Bootstrap modal

@Component({
  selector: 'app-blog-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './blog-list.component.html',
  styleUrl: './blog-list.component.css'
})
export class BlogListComponent implements OnInit {

  blogs$?: Observable<Blog[]>;
  selectedBlog?: Blog;

  constructor(private blogService: BlogService) { }

  ngOnInit(): void {
    // Get all blog posts
    this.blogs$ = this.blogService.getAllBlogPosts();
  }

  openBlogModal(blog: Blog): void {
    this.selectedBlog = blog;
    
    // Wait for Angular to render the modal
    setTimeout(() => {
      const modalElement = document.getElementById('blogModal');
      if (modalElement) {
        const modal = new bootstrap.Modal(modalElement);
        modal.show();
      }
    }, 0);
  }
}
