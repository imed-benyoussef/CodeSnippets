import { Component, OnInit } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { addPhoneNumber, skipPhoneNumber } from '../../store/actions/signup.actions';
import { selectSignupError } from '../../store/selectors/signup.selectors';
import { defaultIfEmpty, map } from 'rxjs';

@Component({
  selector: 'app-phone-page',
  template: `
    <app-phone  (submit)="onSubmit($event)" (skip)="onSkip($event)"></app-phone>
  `
})
export class PhonePageComponent implements OnInit {
  errorMessage$: any;
  ngOnInit() {
    history.pushState(null, '', location.href);
    window.onpopstate = function () {
      history.go(1);
    };
  }
  constructor(private store: Store) { 
    this.errorMessage$ = this.store.pipe(select(selectSignupError('phone')))
    .pipe(
      map(e => e ? e.error_description : ''),  // If e is undefined, map to an empty string
      defaultIfEmpty('')  // Provide a default value if the observable completes without emitting any values
    );
  }

  onSubmit(data: { phone: string }) {
    console.log('setPhoneNumber', data)
    this.store.dispatch(addPhoneNumber(data));
  }
  onSkip($event: void) {
    this.store.dispatch(skipPhoneNumber());
  }
}
