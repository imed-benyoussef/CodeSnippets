import { Component, OnInit } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { verifyEmail } from '../../store/actions/signup.actions';
import { defaultIfEmpty, map, Observable } from 'rxjs';
import { Router } from '@angular/router';
import { selectSignupError, selectUser } from '../../store/selectors/signup.selectors';

@Component({
  selector: 'app-email-verification-page',
  template: `
    <app-email-verification [emailAddress]="(email$ | async)!" (submit)="onSubmit($event)" (back)="onBack()"></app-email-verification>
  `
})
export class EmailVerificationPageComponent implements OnInit {

  email$!: Observable<string | undefined>;
  errorMessage$: Observable<string | null | undefined>;



  onSubmit(data: { code: string }) {
    this.store.dispatch(verifyEmail(data));
  }

  onBack() {
    this.route.navigate(['signup/email']);
  }

  constructor(private store: Store, private route: Router) {
    this.email$ = this.store.pipe(select(selectUser)).pipe(map(u=>u.email));
    this.errorMessage$ = this.store.pipe(select(selectSignupError('email-verification')))
    .pipe(
      map(e => e ? e.error_description : ''),  // If e is undefined, map to an empty string
      defaultIfEmpty('')  // Provide a default value if the observable completes without emitting any values
    );
  }

  ngOnInit() {
    history.pushState(null, '', location.href);
    window.onpopstate = function () {
      history.go(1);
    };
  }
}
