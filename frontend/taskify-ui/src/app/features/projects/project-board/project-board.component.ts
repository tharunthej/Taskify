import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { ProjectService } from '../../../core/services/project.service';
import { AuthService } from '../../../core/services/auth.service';
import { Project } from '../../../models/project.model';
import { ProjectCardComponent } from '../project-card/project-card.component';
import { NavBarComponent } from '../../../core/nav-bar/nav-bar/nav-bar.component';

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
  projects: Project[] = [];
  isLoading = true;

  constructor(
    private projectService: ProjectService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.loadProjects();
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