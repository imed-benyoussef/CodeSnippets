import { Component, Renderer2 } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, RouterOutlet } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html'
  //  animations: [slideInAnimation]
})
export class SignupComponent {

  signupForm: FormGroup;
  prepareRoute(outlet: RouterOutlet) {
    // return outlet && outlet.activatedRouteData && outlet.activatedRouteData['animation'];
  }

  isRtl = false;

  constructor(private fb: FormBuilder, private router: Router, private translate: TranslateService, private renderer: Renderer2) {
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

    this.signupForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      birthdate: ['', Validators.required],
      gender: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      verificationCode: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });

  }


  onSubmit() {
    if (this.signupForm.valid) {
      // Logique pour soumettre le formulaire
      console.log(this.signupForm.value);
    } else {
      // Logique pour gérer les erreurs de validation
      console.log('Formulaire invalide');
    }
  }

  private updateHtmlAttributes(lang: string) {
    const dir = lang === 'ar' ? 'rtl' : 'ltr';
    this.renderer.setAttribute(document.documentElement, 'lang', lang);
    this.renderer.setAttribute(document.documentElement, 'dir', dir);
  }
}
