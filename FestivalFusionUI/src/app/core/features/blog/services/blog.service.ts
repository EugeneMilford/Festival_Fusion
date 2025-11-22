import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Blog } from '../models/blog.model';
import { environment } from '../../../../../environments/environment';
import { AddBlogRequest } from '../models/add-blog-request.model';
import { UpdateBlogRequest } from '../models/update-blog-request.model';

@Injectable({
  providedIn: 'root'
})
export class BlogService {

  constructor(private http: HttpClient) { }

  // Return all Blog Posts
  getAllBlogPosts(): Observable<Blog[]> {
    return this.http.get<Blog[]>(`${environment.apiBaseUrl}/api/blogs?addAuth=true`);
  }

  // Add a new Blog Post
  addBlogPost(model: AddBlogRequest): Observable<Blog> {
    return this.http.post<Blog>(`${environment.apiBaseUrl}/api/blogs?addAuth=true`, model);
  }

  // Get a Blog by id
  getBlogById(id: string): Observable<Blog> {
    return this.http.get<Blog>(`${environment.apiBaseUrl}/api/blogs/${id}?addAuth=true`);
  }

  // Update a Blog Post
  updateBlogPost(id: string, updateBlogRequest: UpdateBlogRequest): Observable<Blog> {
    return this.http.put<Blog>(`${environment.apiBaseUrl}/api/blogs/${id}?addAuth=true`, updateBlogRequest);
  }

  // Remove a Blog
  deleteBlogPost(id: string): Observable<Blog> {
    return this.http.delete<Blog>(`${environment.apiBaseUrl}/api/blogs/${id}?addAuth=true`);
  }
}