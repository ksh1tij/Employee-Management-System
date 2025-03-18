// import { Injectable } from '@angular/core';
// import { HttpClient } from '@angular/common/http';
// import { Observable } from 'rxjs';

// @Injectable({
//   providedIn: 'root'
// })
// export class NotificationService {
//   private apiUrl = 'https://localhost:5246/api/notifications';

//   constructor(private http: HttpClient) {}

//   sendNotification(message: any, recipientUserIds: string[]): Observable<any> {
//     if (typeof message !== 'string') {
//       console.error('Message is not a string:', message);
//       message = String(message); // Convert to string if it's not already
//     }
//     return this.http.post<any>(this.apiUrl, { message, recipientUserIds });
//   }
// }

// import { Injectable } from '@angular/core';
// import * as signalR from '@microsoft/signalr';

// @Injectable({
//   providedIn: 'root'
// })
// export class NotificationService {
//   private hubConnection: signalR.HubConnection;

//   constructor() {
//     this.hubConnection = new signalR.HubConnectionBuilder()
//       .withUrl('http://localhost:5246/notificationHub') // Update the URL to match your backend
//       .build();

//       this.hubConnection.on('ReceiveNotification', (message: any) => {
//         if (typeof message !== 'string') {
//           console.error('Received message is not a string:', message);
//           message = String(message); // Convert to string if it's not already
//         }
//         console.log('Notification received: ', message);
//         alert('Notification: ' + message); // Display the notification
//       });

//     this.hubConnection.start()
//       .then(() => console.log('Connection started'))
//       .catch(err => console.error('Error while starting connection: ' + err));
//   }
// }

import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private hubConnection!: signalR.HubConnection;
  private messageSource = new BehaviorSubject<string>('');
  public currentMessage = this.messageSource.asObservable();

  constructor() {
    this.startConnection();
  }

  private startConnection(){
    this.hubConnection = new signalR.HubConnectionBuilder().withUrl('http://localhost:5246/notificationHub').build();
    this.hubConnection.start().then(() => console.log('Connection started')).catch(err => console.error('Error while starting connection: ', err));
    this.hubConnection.on('ReceiveNotification', (message: string) => {console.log('New Notification: ', message); 
      this.messageSource.next(message);
    });
  }
}