import { Routes } from '@angular/router';
import { TaskBoardComponent } from './features/tasks/task-board/task-board.component';
import { AuthGuard } from './core/guards/auth.guard';
import { ProjectBoardComponent } from './features/projects/project-board/project-board.component';
import { CreateProjectComponent } from './features/projects/create-project/create-project.component';
import { ProjectCardComponent } from './features/projects/project-card/project-card.component';
import { UserProfileComponent } from './features/users/user-profile/user-profile.component';
import { ProjectDetailsComponent } from './features/projects/project-details/project-details.component';
import { HomeComponent } from './home/home/home.component';
import { NavBarComponent } from './core/nav-bar/nav-bar/nav-bar.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'tasks', component: TaskBoardComponent, canActivate: [AuthGuard] },
  { path: 'projects', component: ProjectBoardComponent, canActivate: [AuthGuard], children: [
    { path: '', component: NavBarComponent, outlet: 'navbar' } 
  ] },
  // { path: 'projects/:id', component: ProjectCardComponent, canActivate: [AuthGuard], children: [
  //   { path: '', component: NavBarComponent, outlet: 'navbar' }
  // ] },
  { path: 'profile', component: UserProfileComponent, canActivate: [AuthGuard] },
  { path: 'projects/:id', component: ProjectDetailsComponent, canActivate: [AuthGuard] }
];