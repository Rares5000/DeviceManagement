import { Component, OnInit, inject, signal } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { DeviceService } from '../../../../core/services/device.service';
import { DeviceType } from '../../../../core/models/device.model';
import { LoadingSpinnerComponent } from '../../../../shared/components/loading-spinner/loading-spinner.component';

@Component({
  selector: 'app-device-form',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatSnackBarModule,
    LoadingSpinnerComponent,
  ],
  templateUrl: './device-form.component.html',
  styleUrls: ['./device-form.component.scss'],
})
export class DeviceFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private deviceService = inject(DeviceService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private snackBar = inject(MatSnackBar);

  isLoading = signal(false);
  isEditMode = signal(false);
  deviceId = signal<number | undefined>(undefined);
  DeviceType = DeviceType;

  form = this.fb.group({
    name: ['', Validators.required],
    manufacturer: ['', Validators.required],
    type: ['' as DeviceType, Validators.required],
    operatingSystem: ['', Validators.required],
    osVersion: ['', Validators.required],
    processor: ['', Validators.required],
    ramAmount: [
      null as number | null,
      [Validators.required, Validators.min(1), Validators.max(64)],
    ],
    serialNumber: ['', Validators.required],
    description: [''],
  });

  ngOnInit() {
    const id = this.route.snapshot.params['id'];
    if (id) {
      this.isEditMode.set(true);
      this.deviceId.set(+id);
      this.loadDevice(+id);
    }
  }

  loadDevice(id: number) {
    this.isLoading.set(true);
    this.deviceService.getById(id).subscribe({
      next: (device) => {
        this.form.patchValue(device);
        this.isLoading.set(false);
      },
      error: (err) => {
        this.snackBar.open(err, 'Close', { duration: 3000 });
        this.isLoading.set(false);
      },
    });
  }

  onSubmit() {
    if (this.form.invalid) return;

    this.isLoading.set(true);
    const formValue = this.form.value as any;

    const request$ = this.isEditMode()
      ? this.deviceService.update(this.deviceId()!, formValue)
      : this.deviceService.create(formValue);

    const message = this.isEditMode()
      ? 'Device updated successfully'
      : 'Device created successfully';

    request$.subscribe({
      next: () => {
        this.snackBar.open(message, 'Close', { duration: 3000 });
        this.router.navigate(['/devices']);
      },
      error: (err) => {
        this.snackBar.open(err, 'Close', { duration: 3000 });
        this.isLoading.set(false);
      },
    });
  }

  goBack() {
    this.router.navigate(['/devices']);
  }
}
