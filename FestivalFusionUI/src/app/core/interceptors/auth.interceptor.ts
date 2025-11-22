import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor() {
    console.log('🎯 AuthInterceptor initialized');
  }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    console.log('🔍 Intercepting:', request.url);
    console.log('🔍 Has addAuth=true?', this.shouldInterceptRequest(request));
    
    if (this.shouldInterceptRequest(request)) {
      // Get token from localStorage
      const token = localStorage.getItem('auth-token');
      console.log('🔑 Token from localStorage:', token ? 'FOUND ✅' : 'NOT FOUND ❌');
      
      if (token) {
        const authRequest = request.clone({
          setHeaders: {
            'Authorization': `Bearer ${token}`
          }
        });
        console.log('✅ Added Authorization header');
        return next.handle(authRequest);
      } else {
        console.warn('⚠️ No token found in localStorage!');
      }
    }
    
    return next.handle(request);
  }

  private shouldInterceptRequest(request: HttpRequest<any>): boolean {
    return request.urlWithParams.indexOf('addAuth=true') > -1;
  }
}