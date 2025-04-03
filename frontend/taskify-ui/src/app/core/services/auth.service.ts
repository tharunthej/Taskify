import { HttpClient, HttpErrorResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, BehaviorSubject, tap, catchError, throwError } from "rxjs";

interface UserData {
  userId: number;
  username: string;
  email: string;
  token: string;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private apiUrl = 'https://localhost:5001/api/auth';
  private currentUserSubject = new BehaviorSubject<UserData | null>(null);
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(private http: HttpClient) {
    const storedUser = localStorage.getItem('currentUser');
    if (storedUser) {
      this.currentUserSubject.next(JSON.parse(storedUser));
    }
  }

  login(credentials: { email: string; password: string }): Observable<UserData> {
    return this.http.post<UserData>(`${this.apiUrl}/login`, credentials).pipe(
      tap(userData => {
        this.setUserData(userData);
        this.currentUserSubject.next(userData);
      })
    );
  }

  register(userData: { 
    username: string; 
    email: string; 
    password: string; 
  }): Observable<UserData> {
    return this.http.post<UserData>(`${this.apiUrl}/register`, userData).pipe(
      catchError((error: HttpErrorResponse) => {
        let errorMsg = 'Registration failed';
        if (error.error?.message) {
          errorMsg = error.error.message;
        }
        return throwError(() => new Error(errorMsg));
      }),
      tap(userData => {
        this.setUserData(userData);
        this.currentUserSubject.next(userData);
      })
    );
  }

  private setUserData(userData: UserData): void {
    localStorage.setItem('currentUser', JSON.stringify(userData));
    localStorage.setItem('auth_token', userData.token);
  }

  getToken(): string | null {
    return localStorage.getItem('auth_token');
  }

  getCurrentUser(): UserData | null {
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
}