import { Component, Input } from '@angular/core';
import { User } from '../../signup.models';

@Component({
  selector: 'app-review',
  template: `
    <div class="review-content">
      <div class="d-flex align-items-center mb-4">
        <div class="avatar-container text-bg-secondary me-3">
          <div class="avatar-initials">{{ getInitials() }}</div>
        </div>
        <div>
          <h4 class="mb-0">{{ fullName }}</h4>
          <p class="text-muted mb-1"><i class="icon icon-mail me-1"></i>{{ email }}</p>
          <p *ngIf="phoneNumber" class="text-muted mb-1"><i class="icon icon-phone me-1"></i>{{ phoneNumber }}</p>
        </div>
      </div>
    </div>
  `,
  styles: [`
      .avatar-container {
      width: 50px;
      height: 50px;
      border-radius: 50%;
      display: flex;
      justify-content: center;
      align-items: center;
      color: #fff;
      font-size: 20px;
    }
    .review-content h4 {
      margin: 0;
      font-weight: bold;
    }
    .text-muted {
      margin: 0;
    }
  `]
})
export class ReviewComponent {
  @Input() userData: User | undefined | null;

  get fullName(): string {
    return `${this.userData?.firstName} ${this.userData?.lastName}`;
  }

  get email() {
    return this.userData?.email;
  }
  
  get phoneNumber() {
    return this.userData?.phoneNumber;
  }
  getInitials(): string {
    if (!this.userData?.firstName || !this.userData?.lastName) return '';
    const nameParts = `${this.userData?.firstName} ${this.userData?.lastName}`.split(' ');
    const initials = nameParts.slice(0, 3).map(n => n.charAt(0)).join('');
    return initials.toUpperCase();
  }
}
