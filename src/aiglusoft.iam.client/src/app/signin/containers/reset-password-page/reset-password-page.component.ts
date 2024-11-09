// src/app/signin/containers/reset-password-page/reset-password-page.component.ts
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Store, select } from '@ngrx/store';
import { Observable } from 'rxjs';
import { ResetPasswordState } from '../../store/reducers/reset-password.reducer';
import * as ResetPasswordActions from '../../store/actions/reset-password.actions';
import * as fromResetPassword from '../../store/selectors/reset-password.selectors';

@Component({
  selector: 'app-reset-password-page',
  templateUrl: './reset-password-page.component.html'
})
export class ResetPasswordPageComponent implements OnInit {
  token!: string;
  loading$!: Observable<boolean>;
  errorMessage$!: Observable<string | null>;

  constructor(private route: ActivatedRoute, private store: Store) { }

  ngOnInit(): void {
    this.token = this.route.snapshot.queryParams['token'];
    this.loading$ = this.store.pipe(select(fromResetPassword.selectResetPasswordLoading));
    this.errorMessage$ = this.store.pipe(select(fromResetPassword.selectResetPasswordError));
  }

  onSubmit(data: { password: string }) {
    console.log(data);
    this.store.dispatch(ResetPasswordActions.resetPassword({ password: data.password, token: this.token }));
  }
}
