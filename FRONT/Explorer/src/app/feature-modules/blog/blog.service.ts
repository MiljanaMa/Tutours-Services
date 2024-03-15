import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PagedResult } from './shared/model/paged-result.model';
import { Blog } from './model/blog.model';
import { environment } from 'src/env/environment';
import { Rating } from './model/rating.model';
import { BlogStatus } from './model/blogstatus-model';

@Injectable({
  providedIn: 'root'
})
export class BlogService {

  constructor(private http: HttpClient) { }

  getBlogsWithStatus(): Observable<PagedResult<Blog>> {
    return this.http.get<PagedResult<Blog>>(`${environment.apiHost}blog/status`);
}

  getBlogStatuses(): Observable<PagedResult<BlogStatus>> {
    return this.http.get<PagedResult<BlogStatus>>(`${environment.apiHost}blog/blogStatus`);
  }

  getBlogs(): Observable<PagedResult<Blog>> {
      return this.http.get<PagedResult<Blog>>(environment.apiHost + 'blog');
  }

  getBlog(id: Number): Observable<Blog> {
    return this.http.get<Blog>(`${environment.apiHost}blog/${id}`);
  }

  addBlog(blog: Blog): Observable<Blog> {
    return this.http.post<Blog>(environment.apiHost + 'blog',blog);
  }

  updateBlog(blog: Blog): Observable<Blog> {
    return this.http.put<Blog>(environment.apiHost + 'blog/'+blog.id,blog);
  }

  deleteBlog(blog: Blog): Observable<Blog> {
    return this.http.delete<Blog>(environment.apiHost + 'blog/'+blog.id);
  }

  addRating(rating: Rating):Observable<Blog>{
    return this.http.post<Blog>(environment.apiHost + 'blog/rate',rating);
  }
}
