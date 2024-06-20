import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-recover-password',
  templateUrl: './recover-password.component.html',
  styleUrls: ['./recover-password.component.scss']
})
export class RecoverPasswordComponent implements OnInit {
  recoverPasswordForm: FormGroup;
  successMessage: string | null = null;
  errorMessage: string | null = null;

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private translate: TranslateService
  ) {
    this.recoverPasswordForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]]
    });
  }

  ngOnInit(): void { }

  onRecoverPassword(): void {
    if (this.recoverPasswordForm.invalid) {
      this.recoverPasswordForm.markAllAsTouched();
      return;
    }

    const emailPayload = this.recoverPasswordForm.value;

    this.http.post('/api/v1/account/forgot-password', emailPayload).subscribe({
      next: (response) => {
        console.log('Recover password request sent for email', emailPayload.email);
        this.translate.get('features.auth.components.recoverPassword.SUCCESS_MESSAGE').subscribe((res: string) => {
          this.successMessage = res;
        });
        this.errorMessage = null;
      },
      error: (error) => {
        console.error('Password recovery failed', error);
        this.translate.get('features.auth.components.recoverPassword.ERROR_MESSAGE').subscribe((res: string) => {
          this.errorMessage = error.error?.message || res;
        });
        this.successMessage = null;
      }
    });
  }
}
