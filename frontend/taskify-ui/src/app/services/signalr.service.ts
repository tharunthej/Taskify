// signalr.service.ts
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { environment } from '../../environments/environment';
import { AuthService } from '../core/services/auth.service';
import { TaskItem } from '../models/task.model';

@Injectable({ providedIn: 'root' })
export class SignalRService {
  private hubConnection?: HubConnection;

  constructor(private authService: AuthService) {}

  startConnection(): void {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${environment.apiUrl}/taskhub`, {
        accessTokenFactory: () => this.authService.getToken() || ''
      })
      .withAutomaticReconnect([0, 2000, 5000, 10000, 30000])
      .build();

    this.hubConnection.start()
      .then(() => console.log('SignalR Connected'))
      .catch(err => console.error('SignalR Connection Error:', err));
  }

  listenForTaskUpdates(callback: (task: TaskItem) => void): void {
    this.hubConnection?.on('TaskUpdated', callback);
  }

  triggerTaskUpdate(task: TaskItem): void {
    this.hubConnection?.invoke('UpdateTask', task)
      .catch(err => console.error('SignalR Update Error:', err));
  }

  stopConnection(): void {
    this.hubConnection?.stop()
      .then(() => console.log('SignalR Disconnected'))
      .catch(err => console.error('SignalR Disconnect Error:', err));
  }
}