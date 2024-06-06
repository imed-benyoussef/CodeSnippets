import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LockscreenComponent } from './lockscreen.component';
import { SharedModule } from '../../../../shared/shared.module';
import { TranslateModule } from '@ngx-translate/core';

describe('LockscreenComponent', () => {
  let component: LockscreenComponent;
  let fixture: ComponentFixture<LockscreenComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [LockscreenComponent],
      imports:[SharedModule, TranslateModule.forRoot()]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(LockscreenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
