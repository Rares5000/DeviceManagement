import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavbarComponent } from './core/components/navbar/navbar.component';
import { AuthService } from './core/services/auth.service';
import { inject } from '@angular/core';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NavbarComponent],
  template: `
    @if (authService.isAuthenticated()) {
      <app-navbar />
    }
    <router-outlet />
  `,
})
export class AppComponent {
  authService = inject(AuthService);
}
