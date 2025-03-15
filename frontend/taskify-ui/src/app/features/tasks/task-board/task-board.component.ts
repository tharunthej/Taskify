import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CdkDragDrop, CdkDrag, CdkDropList, moveItemInArray } from '@angular/cdk/drag-drop';
import { TaskService } from '../../../services/task.service';
import { TaskItem } from '../../../models/task.model';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-task-board',
  standalone: true,
  imports: [CommonModule, CdkDropList, CdkDrag, MatCardModule, MatButtonModule],
  templateUrl: './task-board.component.html',
  styleUrls: ['./task-board.component.scss']
})
export class TaskBoardComponent implements OnInit {
  tasks: TaskItem[] = [];
  statuses = ['To Do', 'In Progress', 'Done'];

  constructor(private taskService: TaskService) {}

  ngOnInit(): void {
    this.loadTasks();
  }

  loadTasks(): void {
    this.taskService.getTasks().subscribe(tasks => this.tasks = tasks);
  }

  getTasksByStatus(status: string): TaskItem[] {
    return this.tasks.filter((t: TaskItem) => 
      t.statusId === this.getStatusId(status)
    );
  }

  onDrop(event: CdkDragDrop<TaskItem[]>): void {
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      const task = event.previousContainer.data[event.previousIndex];
      task.statusId = this.getStatusId(event.container.id);
      this.taskService.updateTask(task).subscribe(() => this.loadTasks());
    }
  }

  private getStatusId(statusName: string): number {
    return this.statuses.indexOf(statusName) + 1;
  }
}