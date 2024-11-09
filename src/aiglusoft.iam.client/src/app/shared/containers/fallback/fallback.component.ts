import { Component, Inject, OnInit } from '@angular/core';
import { APP_NAME } from './fallback.module';

@Component({
  selector: 'asui-fallback',
  templateUrl: './fallback.component.html'
})
export class FallbackComponent implements OnInit {
  appName: string = '';

  constructor(@Inject(APP_NAME) private appNameToken: string) { }

  ngOnInit(): void {
    this.appName = this.appNameToken;
  }
}
