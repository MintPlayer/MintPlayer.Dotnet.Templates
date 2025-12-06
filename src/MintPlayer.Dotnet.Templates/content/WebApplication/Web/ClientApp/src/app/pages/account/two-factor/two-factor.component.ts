import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../../../services/account.service';
//#if (UseNgxTranslate)
import { TranslateModule } from '@ngx-translate/core';
//#endif

@Component({
  selector: 'app-two-factor',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
//#if (UseNgxTranslate)
    TranslateModule,
//#endif
  ],
  templateUrl: './two-factor.component.html',
  styleUrl: './two-factor.component.scss'
})
export class TwoFactorComponent implements OnInit {
  twoFactorEnabled = false;
  setupData: { sharedKey: string; authenticatorUri: string } | null = null;
  verificationCode = '';
  errorMessage = '';
  successMessage = '';
  loading = false;
  qrCodeDataUrl = '';

  constructor(private accountService: AccountService) {}

  async ngOnInit() {
    await this.loadStatus();
  }

  async loadStatus() {
    try {
      const user = await this.accountService.getCurrentUser();
      this.twoFactorEnabled = user?.twoFactorEnabled || false;
    } catch (error) {
      // Handle error
    }
  }

  async setupTwoFactor() {
    this.loading = true;
    this.errorMessage = '';

    try {
      this.setupData = await this.accountService.setupTwoFactor();
      await this.generateQrCode(this.setupData.authenticatorUri);
    } catch (error: any) {
      this.errorMessage = error.message || 'Failed to setup 2FA';
    } finally {
      this.loading = false;
    }
  }

  async generateQrCode(uri: string) {
    // Dynamic import of qrcode library
    const QRCode = await import('qrcode');
    this.qrCodeDataUrl = await QRCode.toDataURL(uri);
  }

  async verifyAndEnable() {
    this.loading = true;
    this.errorMessage = '';
    this.successMessage = '';

    try {
      await this.accountService.verifyTwoFactor(this.verificationCode);
      this.twoFactorEnabled = true;
      this.setupData = null;
      this.verificationCode = '';
      this.successMessage = 'Two-factor authentication enabled successfully';
    } catch (error: any) {
      this.errorMessage = error.message || 'Invalid verification code';
    } finally {
      this.loading = false;
    }
  }

  async disableTwoFactor() {
    this.loading = true;
    this.errorMessage = '';
    this.successMessage = '';

    try {
      await this.accountService.disableTwoFactor();
      this.twoFactorEnabled = false;
      this.successMessage = 'Two-factor authentication disabled';
    } catch (error: any) {
      this.errorMessage = error.message || 'Failed to disable 2FA';
    } finally {
      this.loading = false;
    }
  }
}
