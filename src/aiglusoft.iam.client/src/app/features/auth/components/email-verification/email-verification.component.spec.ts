import { ComponentFixture, TestBed } from '@angular/core/testing';
import { EmailVerificationComponent } from './email-verification.component';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { HttpClient } from '@angular/common/http';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { SharedModule } from '../../../../shared/shared.module';

// Factory function for creating TranslateHttpLoader
export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http, 'assets/i18n/', '.json');
}

describe('EmailVerificationComponent', () => {
  let component: EmailVerificationComponent;
  let fixture: ComponentFixture<EmailVerificationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [EmailVerificationComponent],
      imports:[SharedModule, TranslateModule.forRoot()]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EmailVerificationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
