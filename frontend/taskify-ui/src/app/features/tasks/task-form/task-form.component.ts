import { Component, Inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { TaskService } from '../../../services/task.service';
import { TaskItem } from '../../../models/task.model';

@Component({
  selector: 'app-task-form',
  standalone: true,
  imports: [
    FormsModule,
    MatDialogModule,
    MatInputModule,
    MatFormFieldModule,
    MatButtonModule
  ],
  templateUrl: './task-form.component.html',
  styleUrls: ['./task-form.component.scss']
})
export class TaskFormComponent {
  task: Partial<TaskItem> = {
    title: '',
    description: '',
    statusId: 1 // Default to "To Do"
  };

  constructor(
    private taskService: TaskService,
    public dialogRef: MatDialogRef<TaskFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { projectId: number }
  ) {}

  onSubmit(): void {
    const newTask = {
      ...this.task,
      projectId: this.data.projectId,
      createdBy: 1 // Replace with actual user ID from auth
    };

    this.taskService.createTask(newTask as TaskItem).subscribe({
      next: () => this.dialogRef.close(true),
      error: (err) => console.error('Task creation failed:', err)
    });
  }
}