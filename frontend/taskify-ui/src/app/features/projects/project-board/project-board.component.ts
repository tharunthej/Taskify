import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';
import { ProjectService } from '../../../core/services/project.service';
import { AuthService } from '../../../core/services/auth.service';
import { Project, ProjectResponse } from '../../../models/project.model';
import { ProjectCardComponent } from '../project-card/project-card.component';
import { NavBarComponent } from '../../../core/nav-bar/nav-bar/nav-bar.component';
import { CreateProjectComponent } from '../create-project/create-project.component';

@Component({
  selector: 'app-project-board',
  standalone: true,
  templateUrl: './project-board.component.html',
  styleUrls: ['./project-board.component.scss'],
  imports: [
    CommonModule,
    RouterLink,
    ProjectCardComponent,
    MatProgressSpinnerModule,
    MatIconModule,
    NavBarComponent
  ]
})
export class ProjectBoardComponent implements OnInit {
  projects: ProjectResponse[] = [];
  isLoading = true;

  constructor(
    private projectService: ProjectService,
    private dialog: MatDialog,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.loadProjects();
  }

  openCreateProjectDialog(): void {
    const dialogRef = this.dialog.open(CreateProjectComponent, {
      width: '600px',
      maxWidth: '90vw',
      panelClass: 'project-dialog-container'
    });
  
    dialogRef.afterClosed().subscribe(result => {
      if (result === 'created') {
        this.loadProjects(); // Refresh the project list
      }
    });
  }
  

  private loadProjects(): void {
    this.projectService.getAllProjects().subscribe({
      next: (projects) => {
        this.projects = projects;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error loading projects:', err);
        this.isLoading = false;
      }
    });
  }
}