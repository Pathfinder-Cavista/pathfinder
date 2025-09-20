import { computed, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { tap } from 'rxjs';
import { ApiResponseModel } from '../../../models/common/api-response.model';
import { CurrentUserResponseModel, LoginResponseModel } from '../../../models/auth/auth.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly _accessToken = signal<string | null>(localStorage.getItem(environment.accessTokenPhrase));
  private readonly _refreshToken = signal<string | null>(localStorage.getItem(environment.refreshTokenPhrase));
  private readonly _apiBaseUrl = environment.apiBaseUrl;
  private readonly _currentUser = signal<CurrentUserResponseModel | null>(null);
  
  readonly currentUser = this._currentUser.asReadonly();
  isLoggedIn = computed(() => !!this._accessToken());

  constructor(private http: HttpClient, private router: Router) { }

  login(formDetails: {}) {
    return this.http.post<ApiResponseModel<LoginResponseModel>>(`${this._apiBaseUrl}/api/account/login`, formDetails).pipe(
      tap((response) => {
        if (response.success) {
          this.setTokens(response.data.accessToken, response.data.refreshToken);
        }
      })
    );
  }

  logout() {
    this.clearTokens();
    this.router.navigate(['/']);
  }

  refreshToken() {
    const token = this.getRefreshToken();
    return this.http.put<ApiResponseModel<LoginResponseModel>>(`${this._apiBaseUrl}/api/account/refresh`, { token }).pipe(
      tap((response) => {
        if (response.success) {
          this.setTokens(response.data.accessToken, response.data.refreshToken);
        }
      })
    );
  }

  fetchCurrentUser() {
    return this.http.get<ApiResponseModel<CurrentUserResponseModel>>(`${this._apiBaseUrl}/api/users/me`).pipe(
      tap((response) => {
        if (response.success) {
          this._currentUser.set(response.data);
        }
      })
    );
  }

  getAccessToken() {
    return this._accessToken();
  }

  getRefreshToken() {
    return this._refreshToken();
  }

  private setTokens(access: string, refresh: string) {
    this._accessToken.set(access);
    this._refreshToken.set(refresh);
    localStorage.setItem(environment.accessTokenPhrase, access);
    localStorage.setItem(environment.refreshTokenPhrase, refresh);
  }

  private clearTokens() {
    this._accessToken.set(null);
    this._refreshToken.set(null);
    this._currentUser.set(null);
    localStorage.removeItem(environment.accessTokenPhrase);
    localStorage.removeItem(environment.refreshTokenPhrase);
  }
}
