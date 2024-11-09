import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { selectNameSet } from '../store/selectors/signup.selectors';

export const nameSetGuard: CanActivateFn = (route, state) => {
  const store = inject(Store);
  const router = inject(Router);

  return store.select(selectNameSet).pipe(
    take(1),
    map(nameSet => {
      if (!nameSet) {
        router.navigate(['signup/name'], { relativeTo: router.routerState.root });
        return false;
      }
      return true;
    })
  );
};
