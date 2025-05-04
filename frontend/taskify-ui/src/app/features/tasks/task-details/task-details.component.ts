// task-details.component.ts
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TaskService } from '../../../services/task.service';
import { TaskItem, Comment, Attachment, TaskStatus, TaskPriority } from '../../../models/task.model';
import { DatePipe } from '@angular/common';
import { FormControl, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-task-details',
  standalone: true,
  imports: [
    MatCardModule,
    MatChipsModule,
    MatIconModule,
    MatListModule,
    MatInputModule,
    MatButtonModule,
    ReactiveFormsModule,
    DatePipe
  ],
  templateUrl: './task-details.component.html',
  styleUrls: ['./task-details.component.scss']
})
export class TaskDetailsComponent implements OnInit {
  task!: TaskItem;
  commentControl = new FormControl('', [Validators.required]);
  statuses: TaskStatus[] = [];
  priorities: TaskPriority[] = [];
  taskStatusName = '';
  taskPriorityName = '';
  
  // Mock data - replace with actual status/priority names from service
  statusColors: { [key: number]: string } = {
    1: '#4a9eff', // To Do
    2: '#ffb74d', // In Progress
    3: '#66bb6a'  // Done
  };
  
  priorityColors: { [key: number]: string } = {
    1: '#f44336', // High
    2: '#ff9800', // Medium
    3: '#4caf50'  // Low
  };

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private taskService: TaskService
  ) { }

  async ngOnInit(): Promise<void> {
    const taskId = this.route.snapshot.params['id'];
    await this.loadTaskDetails(taskId);
    await this.loadStatusAndPriorities();
  }

  async loadTaskDetails(taskId: number) {
    try {
      const task = await this.taskService.getTaskById(taskId).toPromise();
      if (task) {
        this.task = task;
      } else {
        throw new Error('Task not found');
      }
    } catch (error) {
      console.error('Error loading task details:', error);
      // Handle error (e.g., redirect or show message)
    }
  }

  async loadStatusAndPriorities() {
    try {
      this.statuses = await this.taskService.getStatuses();
      this.priorities = await this.taskService.getPriorities();
      
      // Map names from service data
      this.taskStatusName = this.statuses.find(s => s.id === this.task.statusId)?.status || 'Unknown';
      this.taskPriorityName = this.priorities.find(p => p.id === this.task.priorityId)?.priorityLevel || 'Unknown';
    } catch (error) {
      console.error('Error loading metadata:', error);
    }
  }

  addComment() {
    if (this.commentControl.valid && this.commentControl.value) {
      const newComment: Comment = {
        id: 0,
        content: this.commentControl.value, // Now guaranteed to be string
        createdAt: new Date(),
        userId: 1, // Replace with actual user ID
        user: { id: 1, username: 'Current User' }
      };
      
      this.task.comments = this.task.comments || [];
      this.task.comments.push(newComment);
      this.commentControl.reset();
    }
  }


  getFileIcon(type: string): string {
    const icons: { [key: string]: string } = {
      'pdf': 'picture_as_pdf',
      'image': 'image',
      'doc': 'description',
      'default': 'insert_drive_file'
    };
    return icons[type] || icons['default'];
  }
}