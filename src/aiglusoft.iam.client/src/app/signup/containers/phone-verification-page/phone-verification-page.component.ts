import { Component, OnInit } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { verifyPhoneCode } from '../../store/actions/signup.actions';
import { selectSignupError, selectUser } from '../../store/selectors/signup.selectors';
import { defaultIfEmpty, map, Observable } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-phone-verification-page',
  template: `
    <app-phone-verification [phoneNumber]="(phoneNumber$ | async)!" (back)="onBack()" (submit)="onSubmit($event)"></app-phone-verification>
  `
})
export class PhoneVerificationPageComponent implements OnInit {

  errorMessage$: any;
  phoneNumber$: Observable<string | undefined>;

  ngOnInit() {
    history.pushState(null, '', location.href);
    window.onpopstate = function () {
      history.go(1);
    };
  }
  constructor(private store: Store, private route: Router) {
    this.phoneNumber$ = this.store.pipe(select(selectUser))
      .pipe(
        map(e => e ? e.phoneNumber : ''),  // If e is undefined, map to an empty string
        defaultIfEmpty('')  // Provide a default value if the observable completes without emitting any values
      );

      this.errorMessage$ = this.store.pipe(select(selectSignupError('phone-verification')))
        .pipe(
          map(e => e ? e.error_description : ''),  // If e is undefined, map to an empty string
          defaultIfEmpty('')  // Provide a default value if the observable completes without emitting any values
        );
  }

  onSubmit(data: { code: string }) {
    console.log("verifyPhoneCode", data)
    this.store.dispatch(verifyPhoneCode(data));
  }
  onBack() {
    this.route.navigate(['signup/phone']);
  }
}
