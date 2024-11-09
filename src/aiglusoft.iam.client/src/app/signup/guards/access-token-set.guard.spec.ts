import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { accessTokenSetGuard } from './access-token-set.guard';

describe('accessTokenSetGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => accessTokenSetGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
