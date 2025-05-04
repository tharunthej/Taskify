import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { Router } from '@angular/router';
import { Project, ProjectResponse } from '../../../models/project.model';

@Component({
  selector: 'app-project-card',
  standalone: true,
  templateUrl: './project-card.component.html',
  styleUrls: ['./project-card.component.scss'],
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
]
})
export class ProjectCardComponent {
  @Input() project!: ProjectResponse;

  constructor(private router: Router) {}

  navigateToProject(): void {
    this.router.navigate(['/projects', this.project.id]);
  }
}