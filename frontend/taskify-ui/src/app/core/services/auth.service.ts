import { HttpClient, HttpErrorResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, BehaviorSubject, tap, catchError, throwError } from "rxjs";
import { User, AuthUser } from "../../models/user.model";

@Injectable({ providedIn: 'root' })
export class AuthService {
  private apiUrl = 'http://localhost:5089/api/auth';
  private currentUserSubject = new BehaviorSubject<User | null>(null);
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(private http: HttpClient) {
    const storedUser = localStorage.getItem('currentUser');
    if (storedUser) {
      this.currentUserSubject.next(JSON.parse(storedUser));
    }
  }

  login(credentials: { email: string; password: string }): Observable<AuthUser> {
    return this.http.post<AuthUser>(`${this.apiUrl}/login`, credentials).pipe(
      tap(userData => {
        this.setUserData(userData);
        this.currentUserSubject.next(userData);
      }),
      catchError(this.handleError)
    );
  }

  register(userData: { 
    username: string; 
    email: string; 
    password: string; 
  }): Observable<AuthUser> {
    return this.http.post<AuthUser>(`${this.apiUrl}/register`, userData).pipe(
      tap(userData => {
        this.setUserData(userData);
        this.currentUserSubject.next(userData);
      }),
      catchError(this.handleError)
    );
  }

  private setUserData(userData: AuthUser): void {
    localStorage.setItem('currentUser', JSON.stringify(userData));
    if (userData.token) {
      localStorage.setItem('auth_token', userData.token);
    }
  }

  getToken(): string | null {
    return localStorage.getItem('auth_token');
  }

  getCurrentUser(): User | null {
    return this.currentUserSubject.value;
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }

  logout(): void {
    localStorage.removeItem('currentUser');
    localStorage.removeItem('auth_token');
    this.currentUserSubject.next(null);
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'An unknown error occurred';
    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = `Error: ${error.error.message}`;
    } else {
      // Server-side error
      if (error.error?.message) {
        errorMessage = error.error.message;
      } else {
        errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
      }
    }
    return throwError(() => new Error(errorMessage));
  }

  updateCurrentUser(user: User): void {
    localStorage.setItem('currentUser', JSON.stringify(user));
    this.currentUserSubject.next(user);
  }
}