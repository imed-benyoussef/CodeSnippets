import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { take, map } from 'rxjs';
import { selectGeneralInfoSet } from '../store/selectors/signup.selectors';

export const generalInfoSetGuard: CanActivateFn = (route, state) => {
  const store = inject(Store);
  const router = inject(Router);

  return store.select(selectGeneralInfoSet).pipe(
    take(1),
    map(nameSet => {
      if (!nameSet) {
        router.navigate(['signup/general-info'], { relativeTo: router.routerState.root });
        return false;
      }
      return true;
    })
  );
};
