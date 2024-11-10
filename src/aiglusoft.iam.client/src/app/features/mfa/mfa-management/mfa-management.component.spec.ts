import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MfaManagementComponent } from './mfa-management.component';

describe('MfaManagementComponent', () => {
  let component: MfaManagementComponent;
  let fixture: ComponentFixture<MfaManagementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [MfaManagementComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MfaManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
