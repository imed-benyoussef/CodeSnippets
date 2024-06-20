import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.scss']
})
export class SigninComponent implements OnInit {
  returnUrl: string | null = null;
  signinForm: FormGroup;
  errorMessage: string | null = null; // Variable pour stocker le message d'erreur

  constructor(
    private http: HttpClient,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private translate: TranslateService
  ) {
    this.signinForm = this.fb.group({
      username: ['', [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      rememberMe: [false, []]
    });
  }

  ngOnInit(): void {
    this.route.queryParamMap.subscribe(params => {
      const encodedReturnUri = params.get('ReturnUrl');
      if (encodedReturnUri) {
        this.returnUrl = decodeURIComponent(encodedReturnUri);
        console.log('Decoded Return URI:', this.returnUrl);
      }
    });
  }

  onSignin(): void {
    if (this.signinForm.invalid) {
      this.signinForm.markAllAsTouched();
      return;
    }

    const loginPayload = this.signinForm.value;

    this.http.post('/api/v1/account/login', loginPayload).subscribe({
      next: (response) => {
        console.log('Login successful', response);
        if (this.returnUrl) {
          window.location.href = this.returnUrl;
        } else {
          window.location.href = '/dashboard'; // Open the default route if no returnUri is provided
        }
      },
      error: (error) => {
        console.error('Login failed', error);
        this.translate.get('features.auth.components.signin.LOGIN_FAILED').subscribe((res: string) => {
          this.errorMessage = error.error.errorDescription || res || 'An error occurred during login.';
        });
      }
    });
  }
}
