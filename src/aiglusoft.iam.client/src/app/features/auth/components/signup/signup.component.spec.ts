import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SignupComponent } from './signup.component';
import { SharedModule } from '../../../../shared/shared.module';
import { TranslateModule } from '@ngx-translate/core';

describe('SignupComponent', () => {
  let component: SignupComponent;
  let fixture: ComponentFixture<SignupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [SignupComponent],
      imports:[SharedModule, TranslateModule.forRoot()]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SignupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
