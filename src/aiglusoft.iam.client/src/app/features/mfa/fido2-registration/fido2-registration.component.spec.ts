import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Fido2RegistrationComponent } from './fido2-registration.component';

describe('Fido2RegistrationComponent', () => {
  let component: Fido2RegistrationComponent;
  let fixture: ComponentFixture<Fido2RegistrationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [Fido2RegistrationComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Fido2RegistrationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
