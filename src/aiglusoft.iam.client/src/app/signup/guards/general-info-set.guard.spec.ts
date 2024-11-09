import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { generalInfoSetGuard } from './general-info-set.guard';

describe('generalInfoSetGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => generalInfoSetGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
