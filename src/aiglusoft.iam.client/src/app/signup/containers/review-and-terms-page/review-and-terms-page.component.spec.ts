import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReviewAndTermsPageComponent } from './review-and-terms-page.component';

describe('ReviewAndTermsPageComponent', () => {
  let component: ReviewAndTermsPageComponent;
  let fixture: ComponentFixture<ReviewAndTermsPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ReviewAndTermsPageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReviewAndTermsPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
