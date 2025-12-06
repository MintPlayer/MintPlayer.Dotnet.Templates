import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AccountService } from '../../../services/account.service';
//#if (UseNgxTranslate)
import { TranslateModule } from '@ngx-translate/core';
//#endif

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterLink,
//#if (UseNgxTranslate)
    TranslateModule,
//#endif
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  email = '';
  password = '';
  rememberMe = false;
  errorMessage = '';
  loading = false;
//#if (EnableTwoFactor)
  requiresTwoFactor = false;
//#endif

  constructor(
    private accountService: AccountService,
    private router: Router
  ) {}

  async login() {
    this.loading = true;
    this.errorMessage = '';

    try {
      const response = await this.accountService.login({
        email: this.email,
        password: this.password,
        rememberMe: this.rememberMe
      });

//#if (EnableTwoFactor)
      if (response.requiresTwoFactor) {
        this.requiresTwoFactor = true;
        return;
      }
//#endif

      this.router.navigate(['/']);
    } catch (error: any) {
      this.errorMessage = error.message || 'Login failed';
    } finally {
      this.loading = false;
    }
  }
}
