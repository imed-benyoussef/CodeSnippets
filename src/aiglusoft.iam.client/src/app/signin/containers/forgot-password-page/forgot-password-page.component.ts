import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Store, select } from '@ngrx/store';
import { Observable, Subscription } from 'rxjs';
import * as ForgotPasswordActions from '../../store/actions/forgot-password.actions';
import { selectForgotPasswordError, selectForgotPasswordSuccess } from '../../store/selectors/forgot-password.selectors';

@Component({
  selector: 'app-forgot-password-page',
  templateUrl: './forgot-password-page.component.html',
})
export class ForgotPasswordPageComponent implements OnInit, OnDestroy {
  forgotPasswordForm: FormGroup;
  successMessage$: Observable<string | null>;
  errorMessage$: Observable<string | null>;
  private subscription: Subscription = new Subscription();

  constructor(
    private fb: FormBuilder, 
    private store: Store
  ) {
    this.forgotPasswordForm = this.createforgotPasswordForm();
    this.successMessage$ = this.store.pipe(select(selectForgotPasswordSuccess));
    this.errorMessage$ = this.store.pipe(select(selectForgotPasswordError));
  }

  ngOnInit(): void {
    console.log('ForgotPasswordPageComponent initialized');


    // Dispatch actions to clear success and error messages
    this.store.dispatch(ForgotPasswordActions.clearForgotPasswordMessages());

    this.subscription.add(
      this.successMessage$.subscribe(successMessage => {
        if (successMessage) {
          console.log('Success:', successMessage);
        }
      })
    );
    this.subscription.add(
      this.errorMessage$.subscribe(errorMessage => {
        if (errorMessage) {
          console.error('Error:', errorMessage);
        }
      })
    );
  }

  ngOnDestroy(): void {
    console.log('ForgotPasswordPageComponent destroyed');
    this.subscription.unsubscribe();    
  }

  private createforgotPasswordForm(): FormGroup {
    return this.fb.group({
      email: ['', [Validators.required, Validators.email]],
    });
  }

  onRequestRestPassword(): void {
    if (this.forgotPasswordForm.invalid) {
      this.forgotPasswordForm.markAllAsTouched();
      return;
    }

    const { email } = this.forgotPasswordForm.value;
    this.store.dispatch(ForgotPasswordActions.forgotPassword({ email }));
  }
}
