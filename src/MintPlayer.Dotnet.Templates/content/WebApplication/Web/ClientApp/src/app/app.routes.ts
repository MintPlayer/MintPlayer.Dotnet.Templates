import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/home/home.component').then(m => m.HomeComponent)
  },
//#if (UseAnyIdentityProvider)
  {
    path: 'account',
    loadChildren: () => import('./pages/account/account.routes').then(m => m.accountRoutes)
  },
//#endif
  {
    path: '**',
    redirectTo: ''
  }
];
