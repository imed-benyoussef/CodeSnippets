import { Component, OnInit } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { Observable } from 'rxjs';
import { SignupState } from '../../store/reducers/signup.reducer';
import { selectUser } from '../../store/selectors/signup.selectors';
import { User } from '../../signup.models';

@Component({
  selector: 'app-welcome-page',
  template: `
    <div class="welcome-container">
      <h1>{{ 'welcome.title' | translate }}</h1>
      <p>{{ 'welcome.message' | translate:{ firstName: (user$ | async)?.firstName!, lastName: (user$ | async)?.lastName!} }}</p>
    </div>
  `,
  styles: [`
    .welcome-container {
      text-align: center;
      margin-top: 50px;
    }
  `]
})
export class WelcomePageComponent implements OnInit {
  user$!: Observable<User>;

  constructor(private store: Store<SignupState>) {}

  ngOnInit() {
    this.user$ = this.store.pipe(select(selectUser));
  }
}
