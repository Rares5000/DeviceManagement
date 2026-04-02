import { Routes } from '@angular/router';

export const deviceRoutes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./components/device-list/device-list.component').then(
        (m) => m.DeviceListComponent,
      ),
  },
  {
    path: 'new',
    loadComponent: () =>
      import('./components/device-form/device-form.component').then(
        (m) => m.DeviceFormComponent,
      ),
  },
  {
    path: ':id',
    loadComponent: () =>
      import('./components/device-detail/device-detail.component').then(
        (m) => m.DeviceDetailComponent,
      ),
  },
  {
    path: ':id/edit',
    loadComponent: () =>
      import('./components/device-form/device-form.component').then(
        (m) => m.DeviceFormComponent,
      ),
  },
];
