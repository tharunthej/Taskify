import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from '../../core/auth/login/login.component';
import { RegisterComponent } from '../../core/auth/register/register.component';

@Component({
  selector: 'app-home',
  standalone: true,
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
  imports: [
    LoginComponent,
    RegisterComponent,
    CommonModule
  ]
})
export class HomeComponent {
  showLogin = true;
}