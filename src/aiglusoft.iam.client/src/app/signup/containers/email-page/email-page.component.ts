import { Component, OnInit } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { checkUserEmail } from '../../store/actions/signup.actions';
import { combineLatest, defaultIfEmpty, map, Observable } from 'rxjs';
import { selectSignupError, selectUser } from '../../store/selectors/signup.selectors';

@Component({
  selector: 'app-email-page',
  template: `
    <app-email [value]="email$  | async" [errorMessage]="errorMessage$ | async" (submit)="onSubmit($event)"></app-email>
  `
})
export class EmailPageComponent  {

  errorMessage$!: Observable<string | undefined | null>;
  email$: Observable<string | undefined>;

  onSubmit(data: { email: string }) {
    this.store.dispatch(checkUserEmail(data));
  }
  constructor(private store: Store) {
    this.email$ = this.store.pipe(select(selectUser)).pipe(map(u => u.email));
    this.errorMessage$ = this.store.pipe(select(selectSignupError('email')))
      .pipe(
        map(e => e ? e.error_description : ''),  // If e is undefined, map to an empty string
        defaultIfEmpty('')  // Provide a default value if the observable completes without emitting any values
      );
  }

 
}
