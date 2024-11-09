import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-unauthorized',
  template: `
    <div class="container mt-5">
      <div class="row justify-content-center">
        <div class="col-md-8">
          <div class="card text-center">
            <div class="card-body">
              <h1 class="display-4">401</h1>
              <p class="lead">{{ 'unauthorized.message' | translate }}</p>
              <button class="btn btn-primary" (click)="goToLogin()">{{ 'unauthorized.login' | translate }}</button>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .card {
      border: none;
      box-shadow: 0 4px 8px rgba(0,0,0,0.1);
    }
    .card-body {
      padding: 2rem;
    }
    .display-4 {
      font-size: 4rem;
    }
  `]
})
export class UnauthorizedComponent {
  constructor(private router: Router) {}

  goToLogin() {
    this.router.navigate(['/signin/login']);
  }
}
