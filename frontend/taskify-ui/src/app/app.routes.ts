import { Routes } from '@angular/router';
import { TaskBoardComponent } from './features/tasks/task-board/task-board.component';
import { LoginComponent } from './core/auth/login/login.component';
import { RegisterComponent } from './core/auth/register/register.component';
import { AuthGuard } from './core/guards/auth.guard';
import { reverseAuthGuard } from './core/guards/reverse-auth.guard';
import { ProjectBoardComponent } from './features/projects/project-board/project-board.component';
import { CreateProjectComponent } from './features/projects/create-project/create-project.component';

export const routes: Routes = [
  { path: 'tasks', component: TaskBoardComponent, canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent, canActivate: [reverseAuthGuard] },
  { path: 'register', component: RegisterComponent, canActivate: [reverseAuthGuard] },
  { path: 'projects', component: ProjectBoardComponent, canActivate: [AuthGuard] },
  { path: 'projects/new', component: CreateProjectComponent },
  { path: '', redirectTo: '/projects', pathMatch: 'full' },
];