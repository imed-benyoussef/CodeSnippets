import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { setGeneralInfo } from '../../store/actions/signup.actions';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-general-info-page',
  template: `
    <app-general-info (submit)="onSubmit($event)"></app-general-info>
  `
})
export class GeneralInfoPageComponent implements OnInit {
  ngOnInit() {
    history.pushState(null, '', location.href);
    window.onpopstate = function () {
      history.go(1);
    };
  }
  constructor(private store: Store) {}


  onSubmit(data: { birthDate: string, gender: string }) {
    this.store.dispatch(setGeneralInfo(data));
  }

}
