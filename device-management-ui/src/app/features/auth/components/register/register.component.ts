import { Component, inject, signal } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { AuthService } from '../../../../core/services/auth.service';
import {
  MatCardFooter,
  MatCardContent,
  MatCardSubtitle,
  MatCardTitle,
  MatCardHeader,
  MatCard,
} from '@angular/material/card';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSnackBarModule,
    MatProgressSpinnerModule,
    RouterLink,
    MatCardFooter,
    MatCardContent,
    MatCardSubtitle,
    MatCardTitle,
    MatCardHeader,
    MatCard,
  ],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);
  private snackBar = inject(MatSnackBar);

  isLoading = signal(false);

  form = this.fb.group({
    name: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]],
    role: ['', Validators.required],
    location: ['', Validators.required],
  });

  onSubmit() {
    if (this.form.invalid) return;

    this.isLoading.set(true);
    this.authService.register(this.form.value as any).subscribe({
      next: () => {
        this.snackBar.open('Account created successfully!', 'Close', {
          duration: 3000,
        });
        this.router.navigate(['/devices']);
      },
      error: (err) => {
        this.snackBar.open(err, 'Close', { duration: 3000 });
        this.isLoading.set(false);
      },
    });
  }
}
