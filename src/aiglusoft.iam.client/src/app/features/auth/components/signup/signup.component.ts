import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.scss']
})
export class SignupComponent implements OnInit {
  signupForm: FormGroup;
  errorMessage: string | null = null;

  constructor(private fb: FormBuilder, private http: HttpClient) {
    this.signupForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      username: ['', [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      agreeTerms: [false, [Validators.requiredTrue]]
    });
  }

  ngOnInit(): void {}

  onSignup(): void {
    if (this.signupForm.invalid) {
      this.signupForm.markAllAsTouched();
      return;
    }

    const signupPayload = this.signupForm.value;

    this.http.post('/api/v1/account/register', signupPayload).subscribe({
      next: (response) => {
        console.log('Signup successful', response);
        // Rediriger l'utilisateur ou montrer un message de succès
      },
      error: (error) => {
        console.error('Signup failed', error);
        this.errorMessage = error.error?.errorDescription || 'An error occurred during signup.';
      }
    });
  }
}
