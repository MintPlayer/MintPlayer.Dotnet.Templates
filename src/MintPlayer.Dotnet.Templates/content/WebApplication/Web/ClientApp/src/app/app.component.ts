import { Component } from '@angular/core';
import { RouterOutlet, RouterLink } from '@angular/router';
//#if (UseNgxTranslate)
import { TranslateModule, TranslateService } from '@ngx-translate/core';
//#endif

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    RouterLink,
//#if (UseNgxTranslate)
    TranslateModule,
//#endif
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'MintPlayer.Dotnet.WebApplication';

//#if (UseNgxTranslate)
  constructor(private translate: TranslateService) {
    translate.setDefaultLang('en');
    translate.use('en');
  }

  switchLanguage(lang: string) {
    this.translate.use(lang);
  }
//#endif
}
