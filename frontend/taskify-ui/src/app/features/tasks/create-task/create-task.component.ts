import { Component, Inject, OnInit } from '@angular/core';
import { forkJoin } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { ReactiveFormsModule } from '@angular/forms';
import { TaskService } from '../../../services/task.service';
import { ProjectService } from '../../../core/services/project.service';
import { TaskStatus, TaskPriority } from '../../../models/task.model';
import { User } from '../../../models/user.model';
import { Project, ProjectMember, ProjectResponse } from '../../../models/project.model';

@Component({
  selector: 'app-create-task',
  standalone: true,
  templateUrl: './create-task.component.html',
  styleUrls: ['./create-task.component.scss'],
  imports: [
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    ReactiveFormsModule,
    CommonModule,
    MatIconModule
  ],
})
export class CreateTaskComponent implements OnInit {
  taskForm: FormGroup;
  statuses: TaskStatus[] = [];
  priorities: TaskPriority[] = [];
  projects: ProjectResponse[] = [];
  projectMembers: ProjectMember[] = [];

  constructor(
    private fb: FormBuilder,
    private taskService: TaskService,
    private projectService: ProjectService,
    public dialogRef: MatDialogRef<CreateTaskComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { projectId: number, userId: number }
  ) {
    this.taskForm = this.fb.group({
      title: ['', [Validators.required, Validators.maxLength(150)]],
      description: ['', [Validators.maxLength(1000)]],
      statusId: ['', Validators.required],
      priorityId: ['', Validators.required],
      projectId: [this.data.projectId, Validators.required],
      assignedTo: [''],
      createdBy: [this.data.userId, Validators.required]
    });
  }

  ngOnInit(): void {
    this.loadDropdownData();
  }

  loadDropdownData() {
    forkJoin({
      statuses: this.taskService.getStatuses(),
      priorities: this.taskService.getPriorities(),
      projects: this.projectService.getAllProjects(),
      projectMembers: this.projectService.getProjectMembers(this.data.projectId)
    }).pipe(
      catchError(error => {
        console.error('Error loading dropdown data:', error);
        // Handle error appropriately (e.g., show error message to user)
        throw error;
      })
    ).subscribe(result => {
      this.statuses = result.statuses;
      this.priorities = result.priorities;
      this.projects = result.projects;
      this.projectMembers = result.projectMembers;
    });
  }

  onSubmit() {
    if (this.taskForm.valid) {
      this.dialogRef.close(this.taskForm.value);
    }
  }

  close() {
    this.dialogRef.close();
  }
}