import { Component, OnInit } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { setPassword } from '../../store/actions/signup.actions';
import { defaultIfEmpty, map, Observable } from 'rxjs';
import { selectSignupError } from '../../store/selectors/signup.selectors';

@Component({
  selector: 'app-password-page',
  template: `
    <app-password (submit)="onSubmit($event)"></app-password>
  `
})
export class PasswordPageComponent implements OnInit {
  errorMessage$: Observable<string | null | undefined>;
  ngOnInit() {
    history.pushState(null, '', location.href);
    window.onpopstate = function () {
      history.go(1);
    };
  }
  constructor(private store: Store) {

    this.errorMessage$ = this.store.pipe(select(selectSignupError('password')))
      .pipe(
        map(e => e ? e.error_description : ''),  // If e is undefined, map to an empty string
        defaultIfEmpty('')  // Provide a default value if the observable completes without emitting any values
      );
  }



  async onSubmit(data: { password: string, confirmPassword: string }) {

    this.store.dispatch(setPassword({ encryptedPassword: data.password }));
  }
}
