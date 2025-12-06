import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../../../services/account.service';
import { User } from '../../../entities/user';
//#if (UseNgxTranslate)
import { TranslateModule } from '@ngx-translate/core';
//#endif

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
//#if (UseNgxTranslate)
    TranslateModule,
//#endif
  ],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent implements OnInit {
  user: User | null = null;
  errorMessage = '';
  successMessage = '';
  loading = false;

  // Password change
  currentPassword = '';
  newPassword = '';
  confirmNewPassword = '';

  constructor(private accountService: AccountService) {}

  async ngOnInit() {
    await this.loadProfile();
  }

  async loadProfile() {
    try {
      this.user = await this.accountService.getCurrentUser();
    } catch (error: any) {
      this.errorMessage = error.message || 'Failed to load profile';
    }
  }

  async updateProfile() {
    if (!this.user) return;

    this.loading = true;
    this.errorMessage = '';
    this.successMessage = '';

    try {
      this.user = await this.accountService.updateProfile(this.user);
      this.successMessage = 'Profile updated successfully';
    } catch (error: any) {
      this.errorMessage = error.message || 'Failed to update profile';
    } finally {
      this.loading = false;
    }
  }

  async changePassword() {
    if (this.newPassword !== this.confirmNewPassword) {
      this.errorMessage = 'Passwords do not match';
      return;
    }

    this.loading = true;
    this.errorMessage = '';
    this.successMessage = '';

    try {
      await this.accountService.changePassword(this.currentPassword, this.newPassword);
      this.successMessage = 'Password changed successfully';
      this.currentPassword = '';
      this.newPassword = '';
      this.confirmNewPassword = '';
    } catch (error: any) {
      this.errorMessage = error.message || 'Failed to change password';
    } finally {
      this.loading = false;
    }
  }
}
