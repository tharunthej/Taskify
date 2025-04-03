import { Routes } from "@angular/router";
import { LoginComponent } from "./core/auth/login/login.component";
import { TaskBoardComponent } from "./features/tasks/task-board/task-board.component";
import { RegisterComponent } from "./core/auth/register/register.component";
import { AuthGuard } from "./core/guards/auth.guard";

const routes: Routes = [
    { path: 'login', component: LoginComponent },
    { path: 'tasks', component: TaskBoardComponent, canActivate: [AuthGuard] },
    { path: 'register', component: RegisterComponent },
    { path: '', redirectTo: '/tasks', pathMatch: 'full' },
  ];