import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MfaSelectionComponent } from './mfa-selection.component';

describe('MfaSelectionComponent', () => {
  let component: MfaSelectionComponent;
  let fixture: ComponentFixture<MfaSelectionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [MfaSelectionComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MfaSelectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
