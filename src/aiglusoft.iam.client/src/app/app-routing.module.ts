import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthLayoutComponent } from './core/layouts/auth-layout/auth-layout.component';
import { MainLayoutComponent } from './core/layouts/main-layout/main-layout.component';

const routes: Routes = [
   // Auth routes
  { path: '', component: AuthLayoutComponent, loadChildren: () => import('./features/auth/auth.module').then(m => m.AuthModule) },
  { path: 'dashboard', component: MainLayoutComponent, loadChildren: () => import('./features/dashboard/dashboard.module').then(m => m.DashboardModule) },
 
   // Lazy-loaded feature module routes
  //  { path: 'dashboard', canActivate: [AuthGuard], loadChildren: () => import('./features/dashboard/dashboard.module').then(m => m.DashboardModule) },
   // Redirect to signin by default
  //  { path: '', redirectTo: '/signin', pathMatch: 'full' }, 
   // Wildcard route for a 404 page (not found)
  //  { path: '**', redirectTo: '/signin' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: false })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
