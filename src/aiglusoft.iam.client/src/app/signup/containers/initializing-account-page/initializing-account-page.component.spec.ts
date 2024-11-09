import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InitializingAccountPageComponent } from './initializing-account-page.component';

describe('InitializingAccountPageComponent', () => {
  let component: InitializingAccountPageComponent;
  let fixture: ComponentFixture<InitializingAccountPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [InitializingAccountPageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InitializingAccountPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
