import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { nameSetGuard } from './name-set.guard';

describe('nameSetGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => nameSetGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
