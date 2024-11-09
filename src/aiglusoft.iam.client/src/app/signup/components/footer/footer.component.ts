import { Component, ElementRef, OnDestroy, Renderer2 } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-footer',
  template: `
    <div class="footer bg-light border-top py-3">
      <div class="container d-flex justify-content-between align-items-center">
        <div class="language-selector dropdown">
          <button class="btn btn-link dropdown-toggle" type="button" id="dropdownMenuButton" aria-expanded="false" (click)="toggleMenu()">
            {{ currentLanguage.label }}
          </button>
          <ul class="dropdown-menu" [class.show]="menuOpen" aria-labelledby="dropdownMenuButton">
            <li *ngFor="let lang of languages">
              <a class="dropdown-item" href="javascript:void(0);" (click)="changeLanguage(lang)">
                {{ lang.label }}
              </a>
            </li>
          </ul>
        </div>
        <div class="footer-links">
          <a href="javascript:void(0);" class="text-muted me-3">{{ 'signup.components.footer.help' | translate }}</a>
          <a href="javascript:void(0);" class="text-muted me-3">{{ 'signup.components.footer.privacy' | translate }}</a>
          <a href="javascript:void(0);" class="text-muted">{{ 'signup.components.footer.terms' | translate }}</a>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .dropdown-menu {
      display: none;
    }
    .dropdown-menu.show {
      display: block;
    }
  `]
})
export class FooterComponent implements OnDestroy {
  languages = [
    { code: 'fr', label: 'Français (France)' },
    { code: 'en', label: 'English (US)' },
    { code: 'ar', label: 'العربية' }
  ];
  currentLanguage = this.languages[0];
  menuOpen = false;
  private globalClickListener: () => void;

  constructor(private translate: TranslateService, private renderer: Renderer2, private el: ElementRef) {
    this.currentLanguage = this.languages.find(lang => lang.code === this.translate.currentLang) || this.languages[0];
    this.globalClickListener = this.renderer.listen('document', 'click', this.handleClickOutside.bind(this));
  }

  toggleMenu() {
    this.menuOpen = !this.menuOpen;
  }

  changeLanguage(lang: { code: string, label: string }) {
    this.translate.use(lang.code);
    this.currentLanguage = lang;
    this.menuOpen = false;
  }

  handleClickOutside(event: Event) {
    if (!this.el.nativeElement.contains(event.target)) {
      this.menuOpen = false;
    }
  }

  ngOnDestroy() {
    this.globalClickListener();
  }
}