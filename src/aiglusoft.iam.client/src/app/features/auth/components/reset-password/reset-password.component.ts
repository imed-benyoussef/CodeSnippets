import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { interval, Subscription } from 'rxjs';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent implements OnInit {
  resetPasswordForm: FormGroup;
  successMessage: string | null = null;
  errorMessage: string | null = null;
  token: string | null = null;
  countdown: number | null = null;
  countdownSubscription: Subscription | null = null;

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private route: ActivatedRoute,
    private router: Router,
    private translate: TranslateService
  ) {
    this.resetPasswordForm = this.fb.group({
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required, Validators.minLength(6)]]
    }, { validators: this.passwordsMatchValidator });
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.token = params['token'];
    });
  }

  passwordsMatchValidator(control: AbstractControl): { [key: string]: boolean } | null {
    const password = control.get('password')?.value;
    const confirmPassword = control.get('confirmPassword')?.value;
    if (password !== confirmPassword) {
      return { mismatch: true };
    }
    return null;
  }

  onResetPassword(): void {
    if (this.resetPasswordForm.invalid || !this.token) {
      this.resetPasswordForm.markAllAsTouched();
      return;
    }

    const resetPayload = {
      token: this.token,
      newpassword: this.resetPasswordForm.value.password
    };

    this.http.post('/api/v1/account/reset-password', resetPayload).subscribe({
      next: (response) => {
        this.translate.get('features.auth.components.resetPassword.SUCCESS_MESSAGE').subscribe((res: string) => {
          this.successMessage = res;
          this.startCountdown();
        });
        this.errorMessage = null;
      },
      error: (error) => {
        this.translate.get('features.auth.components.resetPassword.ERROR_MESSAGE').subscribe((res: string) => {
          this.errorMessage = error.error?.message || res;
        });
        this.successMessage = null;
      }
    });
  }

  startCountdown(): void {
    this.countdown = 10;
    this.countdownSubscription = interval(1000).pipe(take(10)).subscribe(() => {
      if (this.countdown !== null) {
        this.countdown--;
        if (this.countdown === 0) {
          this.router.navigate(['/signin']);
        }
      }
    });
  }
}
