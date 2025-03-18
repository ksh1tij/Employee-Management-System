import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { NotificationService } from "./notification.service";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  message: string = '';
  constructor(private notitficationService: NotificationService){}
  // title = 'notification-test';
  ngOnInit(){
    this.notitficationService.currentMessage.subscribe(
      (msg) => this.message = msg
    );
  }
}
  // constructor(public notificationService: NotificationService){}
