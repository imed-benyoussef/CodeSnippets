import { Component, OnInit } from '@angular/core';
import { checkLoginStatus } from './store/actions/login.actions';
import { Store } from '@ngrx/store';

@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styles: ``
})
export class SigninComponent implements OnInit {
  constructor(private store: Store) {}

  ngOnInit() {
    this.store.dispatch(checkLoginStatus());
  }
}
