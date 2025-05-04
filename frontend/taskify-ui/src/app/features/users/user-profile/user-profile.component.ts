import { Component } from '@angular/core';
import { AuthService } from '../../../core/services/auth.service';
import { ProjectService } from '../../../core/services/project.service';
import { User } from '../../../models/user.model';
import { Project, ProjectResponse } from '../../../models/project.model';
import { ProjectCardComponent } from "../../projects/project-card/project-card.component";

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss'],
  imports: [ProjectCardComponent]
})
export class UserProfileComponent {
  user: User | null = null;
  projects: ProjectResponse[] = [];

  constructor(
    private authService: AuthService,
    private projectService: ProjectService
  ) {
    this.user = this.authService.getCurrentUser();
    this.loadProjects();
  }

  private loadProjects(): void {
    if (this.user?.id) {
      this.projectService.getAllProjects().subscribe(projects => {
        this.projects = projects;
      });
    }
  }
}