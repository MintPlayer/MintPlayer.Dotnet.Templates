import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AccountService } from '../../../services/account.service';
//#if (UseNgxTranslate)
import { TranslateModule } from '@ngx-translate/core';
//#endif

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterLink,
//#if (UseNgxTranslate)
    TranslateModule,
//#endif
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  email = '';
  password = '';
  confirmPassword = '';
  firstName = '';
  lastName = '';
  errorMessage = '';
  successMessage = '';
  loading = false;

  constructor(
    private accountService: AccountService,
    private router: Router
  ) {}

  async register() {
    if (this.password !== this.confirmPassword) {
      this.errorMessage = 'Passwords do not match';
      return;
    }

    this.loading = true;
    this.errorMessage = '';
    this.successMessage = '';

    try {
      await this.accountService.register({
        email: this.email,
        password: this.password,
        confirmPassword: this.confirmPassword,
        firstName: this.firstName,
        lastName: this.lastName
      });

//#if (EnableEmailConfirmation)
      this.successMessage = 'Registration successful! Please check your email to confirm your account.';
//#else
      this.router.navigate(['/account/login']);
//#endif
    } catch (error: any) {
      this.errorMessage = error.message || 'Registration failed';
    } finally {
      this.loading = false;
    }
  }
}
