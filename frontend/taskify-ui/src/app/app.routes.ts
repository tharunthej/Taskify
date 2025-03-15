import { Routes } from '@angular/router';
import { TaskBoardComponent } from './features/tasks/task-board/task-board.component';
import { LoginComponent } from './core/auth/login/login.component';
import { AuthGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  { path: 'tasks', component: TaskBoardComponent, canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent },
  { path: '', redirectTo: '/tasks', pathMatch: 'full' },
];