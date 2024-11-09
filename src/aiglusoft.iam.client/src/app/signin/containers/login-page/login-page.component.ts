import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Store, select } from '@ngrx/store';
import * as SigninActions from '../../store/actions/login.actions';
import * as fromLogin from '../../store/selectors/login.selectors';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html'
})
export class LoginPageComponent implements OnInit {
  returnUrl: string | null = null;
  signinForm: FormGroup;
  errorMessage$: Observable<string | null>;

  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private store: Store
  ) {
    this.signinForm = this.fb.group({
      username: ['', [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      rememberMe: [false, []],
    });

    this.errorMessage$ = this.store.pipe(select(fromLogin.selectSigninError));
  }

  ngOnInit(): void {
    this.route.queryParamMap.subscribe((params) => {
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

    const { username, password, rememberMe } = this.signinForm.value;
    this.store.dispatch(SigninActions.LoginActions.login({ username, password, rememberMe, returnUrl: this.returnUrl || '/dashboard' }));
  }
}
