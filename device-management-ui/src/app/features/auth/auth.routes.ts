import { Routes } from '@angular/router';
import { noAuthGuard } from '../../core/guards/no-auth.guard';

export const authRoutes: Routes = [
  {
    path: 'login',
    canActivate: [noAuthGuard],
    loadComponent: () =>
      import('./components/login/login.component').then(
        (m) => m.LoginComponent,
      ),
  },
  {
    path: 'register',
    canActivate: [noAuthGuard],
    loadComponent: () =>
      import('./components/register/register.component').then(
        (m) => m.RegisterComponent,
      ),
  },
];
