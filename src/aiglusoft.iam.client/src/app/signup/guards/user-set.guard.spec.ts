import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { userSetGuard } from './user-set.guard';

describe('userSetGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => userSetGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
