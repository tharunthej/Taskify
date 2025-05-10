import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AuthService } from '../../../core/services/auth.service';
import { ProjectService } from '../../../core/services/project.service';
import { UserService } from '../../../services/user.service';
import { User } from '../../../models/user.model';
import { ProjectResponse } from '../../../models/project.model';
import { EditProfileDialogComponent } from '../edit-profile-dialog/edit-profile-dialog.component';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { NavBarComponent } from '../../../core/nav-bar/nav-bar/nav-bar.component';
import { ProjectCardComponent } from '../../projects/project-card/project-card.component';

@Component({
  selector: 'app-user-profile',
  standalone: true,
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss'],
  imports: [
    CommonModule,
    MatIconModule,
    NavBarComponent,
    ProjectCardComponent
  ]
})
export class UserProfileComponent implements OnInit {
  user: User | null = null;
  projects: ProjectResponse[] = [];

  constructor(
    private authService: AuthService,
    private projectService: ProjectService,
    private userService: UserService,
    private dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.user = this.authService.getCurrentUser();
    this.loadAdminProjects();
    console.log('User:', this.user);
  }

  private loadAdminProjects(): void {
    this.projectService.getProjectsByAdmin().subscribe({
      next: (projects) => this.projects = projects,
      error: (err) => console.error('Error loading projects:', err)
    });
  }

  openEditDialog(): void {
    const dialogRef = this.dialog.open(EditProfileDialogComponent, {
      width: '500px',
      data: { user: { ...this.user } },
      ariaDescribedBy: 'edit-profile-dialog-description', // Add ARIA attributes
      ariaLabelledBy: 'edit-profile-dialog-title',
      autoFocus: false // Prevent auto-focus if causing issues
    });
  
    dialogRef.afterClosed().subscribe(updatedData => {
      if (updatedData) {
        this.userService.updateUser(this.user!.id, updatedData).subscribe({
          next: (updatedUser) => {
            this.user = updatedUser;
            this.authService.updateCurrentUser(updatedUser);
          },
          error: (err) => console.error('Update failed:', err)
        });
      }
    });
  }
}