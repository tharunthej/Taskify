import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CdkDragDrop, CdkDrag, CdkDropList, moveItemInArray } from '@angular/cdk/drag-drop';
import { TaskService } from '../../../services/task.service';
import { TaskItem } from '../../../models/task.model';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { ReplacePipe } from '../../../core/pipes/replace-spaces.pipe';
import { NavBarComponent } from '../../../core/nav-bar/nav-bar/nav-bar.component';
import { CreateTaskComponent } from '../create-task/create-task.component';
import { SignalRService } from '../../../services/signalr.service';

@Component({
  selector: 'app-task-board',
  standalone: true,
  imports: [
    CommonModule, 
    CdkDropList, 
    CdkDrag, 
    ReplacePipe,
    NavBarComponent,
    MatCardModule, 
    MatButtonModule,
    MatIconModule
  ],
  templateUrl: './task-board.component.html',
  styleUrls: ['./task-board.component.scss']
})
export class TaskBoardComponent implements OnInit, OnDestroy {
  tasks: TaskItem[] = [];
  statuses = ['To Do', 'In Progress', 'Done', 'Rejected'];

  constructor(
    private taskService: TaskService,
    private dialog: MatDialog,
    private signalr: SignalRService
  ) {}

  ngOnInit(): void {
    this.loadTasks();
    this.initializeSignalR();
  }

  private initializeSignalR(): void {
    this.signalr.startConnections();
    this.signalr.listenForTaskUpdates((updatedTask: TaskItem) => {
      const index = this.tasks.findIndex(t => t.id === updatedTask.id);
      if (index > -1) {
        // Update task and maintain array reference for change detection
        this.tasks = [
          ...this.tasks.slice(0, index),
          updatedTask,
          ...this.tasks.slice(index + 1)
        ];
      }
    });
  }

  loadTasks(): void {
    this.taskService.getTasks().subscribe(tasks => this.tasks = tasks);
  }

  getTasksByStatus(status: string): TaskItem[] {
    return this.tasks.filter((t: TaskItem) => 
      t.statusId === this.getStatusId(status)
    );
  }

  draggedTask: TaskItem | null = null;
  isProjectAdmin = true; // Set this based on actual user role

  onDrop(event: CdkDragDrop<TaskItem[]>): void {
    this.draggedTask = null;
  
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      const task = event.previousContainer.data[event.previousIndex];
      const newStatusId = this.getStatusId(event.container.id);
      
      // Optimistic UI update
      const updatedTask = { ...task, statusId: newStatusId };
      const index = this.tasks.findIndex(t => t.id === task.id);
      this.tasks[index] = updatedTask;

      // Send update to server and sync with SignalR
      this.taskService.updateTask(updatedTask).subscribe({
        next: () => this.signalr.triggerTaskUpdate(updatedTask),
        error: () => this.loadTasks() // Rollback if error
      });
    }
  }

  onDragStarted(task: TaskItem): void {
    this.draggedTask = task;
  }

  private getStatusId(statusName: string): number {
    return this.statuses.indexOf(statusName) + 1;
  }

  openTaskForm(): void {
    const dialogRef = this.dialog.open(CreateTaskComponent, {
      data: { projectId: 1 } // Pass actual project ID
    });

    dialogRef.afterClosed().subscribe(created => {
      if (created) {
        this.loadTasks();
        this.signalr.triggerTaskUpdate({} as TaskItem); // Refresh all clients
      }
    });
  }

  ngOnDestroy(): void {
    this.signalr.stopConnections();
  }
}