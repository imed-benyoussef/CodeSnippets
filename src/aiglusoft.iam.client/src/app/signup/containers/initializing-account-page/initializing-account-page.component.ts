import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-initializing-account-page',
  template: `
    <div class="loading-overlay d-flex align-items-center justify-content-center">
      
      <div class="card p-4" style="width: 20rem;">
        <div class="card-title">
          <img src="assets/images/logo-dark.png" alt="Aiglusoft Logo" style="width: 100px;">
        </div>
        <div class="card-text ">
          <div class="d-flex flex-row mt-2">
              <div class="spinner-border spinner-border-sm me-2" role="status">
                <span class="visually-hidden">{{ 'signup.common.initializingAccount' | translate }} </span>
              </div>
              <span class="mb-0">{{ 'signup.common.initializingAccount' | translate }}</span>
            </div>
          </div>
      </div>
    </div>
  `,
  styles: [`
    .loading-overlay {
      position: fixed;
      top: 0;
      left: 0;
      width: 100%;
      height: 100%;
      background: rgba(0, 0, 0, 0.5);
      z-index: 1050;
    }
    .position-absolute {
      z-index: 1;
    }
  `]
})
export class InitializingAccountPageComponent implements OnInit {
  ngOnInit() {
    history.pushState(null, '', location.href);
    window.onpopstate = function () {
      history.go(1);
    };
  }
}
