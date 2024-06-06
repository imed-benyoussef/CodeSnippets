import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecoverPasswordComponent } from './recover-password.component';
import { SharedModule } from '../../../../shared/shared.module';
import { TranslateModule } from '@ngx-translate/core';

describe('RecoverPasswordComponent', () => {
  let component: RecoverPasswordComponent;
  let fixture: ComponentFixture<RecoverPasswordComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [RecoverPasswordComponent],
      imports:[SharedModule, TranslateModule.forRoot()]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RecoverPasswordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
