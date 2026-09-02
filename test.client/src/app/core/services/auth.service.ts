import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AuthResult } from '../models/auth/AuthResult';
import { LoginRequest } from '../models/auth/LoginRequest';
import { RegisterRequest } from '../models/auth/RegisterRequest';

const STORAGE_KEY = 'auth';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private authSubject = new BehaviorSubject<AuthResult | null>(this.readStoredAuth());
  auth$ = this.authSubject.asObservable();

  constructor(private http: HttpClient) { }

  login(request: LoginRequest): Observable<AuthResult> {
    return this.http.post<AuthResult>(`${environment.apiUrl}/auth/login`, request)
      .pipe(tap(result => this.setAuth(result)));
  }

  register(request: RegisterRequest): Observable<AuthResult> {
    return this.http.post<AuthResult>(`${environment.apiUrl}/auth/register`, request)
      .pipe(tap(result => this.setAuth(result)));
  }

  logout(): void {
    localStorage.removeItem(STORAGE_KEY);
    this.authSubject.next(null);
  }

  get token(): string | null {
    return this.authSubject.value?.token ?? null;
  }

  get isLoggedIn(): boolean {
    return !!this.authSubject.value;
  }

  get isAdmin(): boolean {
    return this.authSubject.value?.roles.includes('Admin') ?? false;
  }

  private setAuth(result: AuthResult): void {
    localStorage.setItem(STORAGE_KEY, JSON.stringify(result));
    this.authSubject.next(result);
  }

  private readStoredAuth(): AuthResult | null {
    const raw = localStorage.getItem(STORAGE_KEY);
    return raw ? JSON.parse(raw) as AuthResult : null;
  }
}
