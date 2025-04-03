// signalr.service.ts
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { environment } from '../../environments/environment';
import { AuthService } from '../core/services/auth.service';
import { TaskItem } from '../models/task.model';
import { Project } from '../models/project.model';

@Injectable({ providedIn: 'root' })
export class SignalRService {
  private taskHubConnection?: HubConnection;
  private projectHubConnection?: HubConnection;

  constructor(private authService: AuthService) {}

  startConnections(): void {
    this.startTaskHubConnection();
    this.startProjectHubConnection();
  }

  private startTaskHubConnection(): void {
    this.taskHubConnection = new HubConnectionBuilder()
      .withUrl(`${environment.apiUrl}/taskhub`, {
        accessTokenFactory: () => this.authService.getToken() || ''
      })
      .withAutomaticReconnect([0, 2000, 5000, 10000, 30000])
      .build();

    this.taskHubConnection.start()
      .then(() => console.log('Task Hub Connected'))
      .catch(err => console.error('Task Hub Connection Error:', err));
  }

  private startProjectHubConnection(): void {
    this.projectHubConnection = new HubConnectionBuilder()
      .withUrl(`${environment.apiUrl}/projecthub`, {
        accessTokenFactory: () => this.authService.getToken() || ''
      })
      .withAutomaticReconnect([0, 2000, 5000, 10000, 30000])
      .build();

    this.projectHubConnection.start()
      .then(() => console.log('Project Hub Connected'))
      .catch(err => console.error('Project Hub Connection Error:', err));
  }

  // Task Hub Listeners
  listenForTaskUpdates(callback: (task: TaskItem) => void): void {
    this.taskHubConnection?.on('TaskCreated', callback);
    this.taskHubConnection?.on('TaskUpdated', callback);
    this.taskHubConnection?.on('TaskDeleted', (id: number) => {
      callback({ id } as TaskItem); // Handle deletion ID
    });
  }

  // Project Hub Listeners
  listenForProjectUpdates(callback: (project: Project | { id: number }) => void): void {
    this.projectHubConnection?.on('ProjectCreated', callback);
    this.projectHubConnection?.on('ProjectUpdated', callback);
    this.projectHubConnection?.on('ProjectDeleted', (id: number) => {
      callback({ id }); // Handle deletion ID
    });
  }

  listenForMemberUpdates(callback: (data: { projectId: number, userId: number }) => void): void {
    this.projectHubConnection?.on('MemberAdded', callback);
  }

  // Task Hub Methods
  triggerTaskUpdate(task: TaskItem): void {
    this.taskHubConnection?.invoke('UpdateTask', task)
      .catch(err => console.error('Task Update Error:', err));
  }

  // Project Hub Methods
  joinProjectGroup(projectId: number): void {
    this.projectHubConnection?.invoke('JoinProjectGroup', projectId)
      .catch(err => console.error('Join Project Group Error:', err));
  }

  stopConnections(): void {
    this.taskHubConnection?.stop()
      .then(() => console.log('Task Hub Disconnected'))
      .catch(err => console.error('Task Hub Disconnect Error:', err));
    
    this.projectHubConnection?.stop()
      .then(() => console.log('Project Hub Disconnected'))
      .catch(err => console.error('Project Hub Disconnect Error:', err));
  }
}