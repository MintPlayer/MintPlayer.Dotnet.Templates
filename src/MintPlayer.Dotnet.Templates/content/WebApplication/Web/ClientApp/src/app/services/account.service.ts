import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { User } from '../entities/user';

export interface LoginRequest {
  email: string;
  password: string;
  rememberMe: boolean;
}

export interface LoginResponse {
  user: User | null;
  requiresTwoFactor: boolean;
  token?: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
  confirmPassword: string;
  firstName?: string;
  lastName?: string;
}

//#if (EnableTwoFactor)
export interface TwoFactorSetupResponse {
  sharedKey: string;
  authenticatorUri: string;
}
//#endif

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private readonly baseUrl = '/api/v1/account';

  constructor(private http: HttpClient) {}

  async register(request: RegisterRequest): Promise<User> {
    return firstValueFrom(
      this.http.post<User>(`${this.baseUrl}/register`, request)
    );
  }

  async login(request: LoginRequest): Promise<LoginResponse> {
    return firstValueFrom(
      this.http.post<LoginResponse>(`${this.baseUrl}/login`, request)
    );
  }

  async logout(): Promise<void> {
    await firstValueFrom(
      this.http.post<void>(`${this.baseUrl}/logout`, {})
    );
  }

  async getCurrentUser(): Promise<User | null> {
    try {
      return await firstValueFrom(
        this.http.get<User>(`${this.baseUrl}/current`)
      );
    } catch {
      return null;
    }
  }

  async updateProfile(user: User): Promise<User> {
    return firstValueFrom(
      this.http.put<User>(`${this.baseUrl}/profile`, user)
    );
  }

  async changePassword(currentPassword: string, newPassword: string): Promise<void> {
    await firstValueFrom(
      this.http.post<void>(`${this.baseUrl}/change-password`, {
        currentPassword,
        newPassword
      })
    );
  }

//#if (EnableTwoFactor)
  async setupTwoFactor(): Promise<TwoFactorSetupResponse> {
    return firstValueFrom(
      this.http.get<TwoFactorSetupResponse>(`${this.baseUrl}/two-factor/setup`)
    );
  }

  async verifyTwoFactor(code: string): Promise<void> {
    await firstValueFrom(
      this.http.post<void>(`${this.baseUrl}/two-factor/verify`, { code })
    );
  }

  async disableTwoFactor(): Promise<void> {
    await firstValueFrom(
      this.http.post<void>(`${this.baseUrl}/two-factor/disable`, {})
    );
  }
//#endif
}
