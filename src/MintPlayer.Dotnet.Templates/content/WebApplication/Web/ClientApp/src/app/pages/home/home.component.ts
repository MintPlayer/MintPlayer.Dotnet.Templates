import { Component } from '@angular/core';
//#if (UseNgxTranslate)
import { TranslateModule } from '@ngx-translate/core';
//#endif

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
//#if (UseNgxTranslate)
    TranslateModule,
//#endif
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {
}
