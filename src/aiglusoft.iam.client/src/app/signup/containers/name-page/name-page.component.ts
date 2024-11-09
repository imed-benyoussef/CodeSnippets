import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { setName } from '../../store/actions/signup.actions';

@Component({
  selector: 'app-name-page',
  template: `
    <app-name (submit)="onSubmit($event)"></app-name>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class NamePageComponent implements OnInit {
  ngOnInit() {
    history.pushState(null, '', location.href);
    window.onpopstate = function () {
      history.go(1);
    };
  }
  constructor(private store: Store) {}

  async onSubmit(data: { firstName: string, lastName: string }) {

    // const encrypted = await this.encryptionService.encryptData(data.firstName);

    // data.firstName = encrypted;
    
    this.store.dispatch(setName(data));
  }
}
