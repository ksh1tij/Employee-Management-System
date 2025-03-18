import { Injectable, Inject } from '@angular/core';
import { BASE_URL } from './app.config';

@Injectable({
  providedIn: 'root'
})
export class InitService {
  constructor(@Inject(BASE_URL) private baseUrl: string) {}

  init(): Promise<void> {
    return new Promise((resolve, reject) => {
      console.log('BASE_URL in InitService:', this.baseUrl);
      // Perform any other initialization logic here
      resolve();
    });
  }
}