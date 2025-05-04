import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, lastValueFrom } from 'rxjs';
import { TaskItem, TaskStatus, TaskPriority } from '../models/task.model';

@Injectable({ providedIn: 'root' })
export class TaskService {
  private apiUrl = 'https://localhost:5089/api';
  
  constructor(private http: HttpClient) {}

  // Existing task methods
  getTasks(): Observable<TaskItem[]> {
    return this.http.get<TaskItem[]>(`${this.apiUrl}/tasks`);
  }

  getTaskById(id: number): Observable<TaskItem> {
    return this.http.get<TaskItem>(`${this.apiUrl}/tasks/${id}`);
  }

  updateTask(task: TaskItem): Observable<TaskItem> {
    return this.http.put<TaskItem>(`${this.apiUrl}/tasks/${task.id}`, task);
  }

  createTask(task: TaskItem): Observable<TaskItem> {
    return this.http.post<TaskItem>(`${this.apiUrl}/tasks`, task);
  }

  deleteTask(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/tasks/${id}`);
  }

  async getStatuses(): Promise<TaskStatus[]> {
    return lastValueFrom(
      this.http.get<TaskStatus[]>(`${this.apiUrl}/statuses`)
    );
  }
  
  async getPriorities(): Promise<TaskPriority[]> {
    return lastValueFrom(
      this.http.get<TaskPriority[]>(`${this.apiUrl}/priorities`)
    );
  }
}