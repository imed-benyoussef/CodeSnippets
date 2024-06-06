import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AuthLayoutComponent } from './auth-layout.component';
import { AuthCarouselComponent } from '../../shared/components/auth-carousel/auth-carousel.component';
import { RouterModule } from '@angular/router';

describe('AuthLayoutComponent', () => {
  let component: AuthLayoutComponent;
  let fixture: ComponentFixture<AuthLayoutComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AuthLayoutComponent, AuthCarouselComponent],
      imports:[RouterModule]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AuthLayoutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
