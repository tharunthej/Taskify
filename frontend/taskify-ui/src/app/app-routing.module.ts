import { Routes } from "@angular/router";
import { LoginComponent } from "./core/auth/login/login.component";
import { AuthGuard } from "./core/services/auth.guard";

const routes: Routes = [
    { path: 'login', component: LoginComponent },
    { path: 'tasks', component: TaskBoardComponent, canActivate: [AuthGuard] },
    { path: '', redirectTo: '/tasks', pathMatch: 'full' },
  ];