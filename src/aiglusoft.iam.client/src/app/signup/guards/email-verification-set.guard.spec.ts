import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { emailVerificationSetGuard } from './email-verification-set.guard';

describe('emailVerificationSetGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => emailVerificationSetGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
