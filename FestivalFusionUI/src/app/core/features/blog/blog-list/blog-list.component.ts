import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { CommonModule, AsyncPipe, NgFor, NgIf } from '@angular/common';
import { RouterLink } from '@angular/router';
import { BlogService } from '../services/blog.service';
import { Blog } from '../models/blog.model';
import { AuthService } from '../../../features/auth/services/auth.service';

declare var bootstrap: any; // For Bootstrap modal

@Component({
  selector: 'app-blog-list',
  standalone: true,
  imports: [CommonModule, RouterLink, NgIf, NgFor, AsyncPipe],
  templateUrl: './blog-list.component.html',
  styleUrl: './blog-list.component.css'
})
export class BlogListComponent implements OnInit {

  blogs$?: Observable<Blog[]>;
  selectedBlog?: Blog;
  userRoles: string[] = [];

  constructor(
    private blogService: BlogService,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    // Get all blog posts
    this.blogs$ = this.blogService.getAllBlogPosts();
    this.loadUserRoles();
  }

  loadUserRoles(): void {
    const user = this.authService.getUser();
    if (user) {
      this.userRoles = user.roles;
      console.log('👤 User roles:', this.userRoles);
    }
  }

  // Check if user can add blog posts (Writer, Editor)
  canAdd(): boolean {
    return this.userRoles.some(role =>
      ['Writer', 'Editor'].includes(role)
    );
  }

  // Check if user can edit blog posts (Editor, Moderator)
  canEdit(): boolean {
    return this.userRoles.some(role =>
      ['Editor', 'Moderator'].includes(role)
    );
  }

  // Check if user can delete blog posts (Editor only)
  canDelete(): boolean {
    return this.userRoles.includes('Editor');
  }

  openBlogModal(blog: Blog): void {
    this.selectedBlog = blog;

    // Wait for Angular to render the modal element
    setTimeout(() => {
      const modalElement = document.getElementById('blogModal');
      if (modalElement) {
        const modal = new bootstrap.Modal(modalElement);
        modal.show();
      }
    }, 0);
  }
}