import { Component, Renderer2 } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html'
  //  animations: [slideInAnimation]
})
export class SignupComponent {
  prepareRoute(outlet: RouterOutlet) {
    // return outlet && outlet.activatedRouteData && outlet.activatedRouteData['animation'];
  }

  isRtl = false;

  constructor(private router: Router, private translate: TranslateService, private renderer: Renderer2) {
    // Handle language change
    this.translate.onLangChange.subscribe((event) => {
      this.isRtl = event.lang === 'ar';
      this.updateHtmlAttributes(event.lang);
    });

    // Set default language
    const defaultLang = 'fr';
    this.translate.setDefaultLang(defaultLang);
    this.translate.use(defaultLang);
    this.updateHtmlAttributes(defaultLang);

  }

  private updateHtmlAttributes(lang: string) {
    const dir = lang === 'ar' ? 'rtl' : 'ltr';
    this.renderer.setAttribute(document.documentElement, 'lang', lang);
    this.renderer.setAttribute(document.documentElement, 'dir', dir);
  }
}
