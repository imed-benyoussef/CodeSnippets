import { CanActivateFn, Router } from '@angular/router';
import { selectEmailSet } from '../store/selectors/signup.selectors';
import { inject } from '@angular/core';
import { Store } from '@ngrx/store';
import { take, map } from 'rxjs';

export const emailSetGuard: CanActivateFn = (route, state) => {
  const store = inject(Store);
  const router = inject(Router);

  return store.select(selectEmailSet).pipe(
    take(1),
    map(nameSet => {
      if (!nameSet) {
        router.navigate(['signup/email'], { relativeTo: router.routerState.root });
        return false;
      }
      return true;
    })
  );
};
