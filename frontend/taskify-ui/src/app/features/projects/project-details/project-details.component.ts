// project-details.component.ts
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProjectService } from '../../../core/services/project.service';
import { Project, ProjectResponse, TaskBrief } from '../../../models/project.model';
import { TaskItem } from '../../../models/task.model';
import { MatDialog } from '@angular/material/dialog';
import { DatePipe } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatListModule } from '@angular/material/list';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { NavBarComponent } from "../../../core/nav-bar/nav-bar/nav-bar.component";
import { CreateTaskComponent } from '../../tasks/create-task/create-task.component';

@Component({
  selector: 'app-project-details',
  standalone: true,
  templateUrl: './project-details.component.html',
  styleUrls: ['./project-details.component.scss'],
  imports: [
    MatCardModule,
    MatListModule,
    MatChipsModule,
    DatePipe,
    MatIconModule,
    NavBarComponent
],
})
export class ProjectDetailsComponent implements OnInit {
  project!: ProjectResponse;
  tasks: TaskBrief[] = [];
  
  // Define status configuration locally
  statusConfig: { [key: number]: { name: string; color: string } } = {
    1: { name: 'To Do', color: '#4a9eff' },
    2: { name: 'In Progress', color: '#ffb74d' },
    3: { name: 'Done', color: '#66bb6a' }
  };

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private projectService: ProjectService,
    private dialog: MatDialog,
  ) { }

  ngOnInit(): void {
    const projectId = this.route.snapshot.params['id'];
    this.loadProjectDetails(projectId);
  }

  openCreateTaskDialog(): void {
    const dialogRef = this.dialog.open(CreateTaskComponent, {
      width: '720px',
      data: { projectId: this.project.id },
      panelClass: 'task-dialog-container'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result === 'created') {
        this.loadProjectDetails(this.project.id); // Refresh task list
      }
    });
  }

  async loadProjectDetails(projectId: number) {
    try {
      const project = await this.projectService.getProjectById(projectId).toPromise();
      
      if (!project) {
        throw new Error('Project not found');
      }

      this.project = project;
      this.tasks = this.project.tasks || [];
      
    } catch (error) {
      console.error('Error loading project details:', error);
      this.router.navigate(['/not-found']);
    }
  }

  getStatusName(statusId: number): string {
    return this.statusConfig[statusId]?.name || 'Unknown';
  }

  getStatusColor(statusId: number): string {
    return this.statusConfig[statusId]?.color || '#666';
  }

  navigateToTask(taskId: number): void {
    this.router.navigate(['/tasks', taskId]);
  }
}