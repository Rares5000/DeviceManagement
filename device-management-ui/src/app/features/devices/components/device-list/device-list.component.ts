import { Component, inject, signal, computed } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { DeviceService } from '../../../../core/services/device.service';
import { Device } from '../../../../core/models/device.model';
import { ConfirmDialogComponent } from '../../../../shared/components/confirm-dialog/confirm-dialog.component';
import { LoadingSpinnerComponent } from '../../../../shared/components/loading-spinner/loading-spinner.component';

@Component({
  selector: 'app-device-list',
  standalone: true,
  imports: [
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatSnackBarModule,
    MatDialogModule,
    LoadingSpinnerComponent,
    RouterLink,
  ],
  templateUrl: './device-list.component.html',
  styleUrls: ['./device-list.component.scss'],
})
export class DeviceListComponent {
  // inject() în loc de constructor
  private deviceService = inject(DeviceService);
  private router = inject(Router);
  private dialog = inject(MatDialog);
  private snackBar = inject(MatSnackBar);

  // Signals
  isLoading = signal(false);
  devices = signal<Device[]>([]);
  displayedColumns = [
    'name',
    'manufacturer',
    'type',
    'os',
    'ram',
    'serialNumber',
    'assignedTo',
    'actions',
  ];

  // Computed signal - număr de device-uri asignate
  assignedCount = computed(
    () => this.devices().filter((d) => d.assignedUserId).length,
  );

  constructor() {
    this.loadDevices();
  }

  loadDevices() {
    this.isLoading.set(true);
    this.deviceService.getAll().subscribe({
      next: (devices) => {
        this.devices.set(devices);
        this.isLoading.set(false);
      },
      error: (err) => {
        this.snackBar.open(err, 'Close', { duration: 3000 });
        this.isLoading.set(false);
      },
    });
  }

  viewDevice(id: number) {
    this.router.navigate(['/devices', id]);
  }

  editDevice(id: number) {
    this.router.navigate(['/devices', id, 'edit']);
  }

  deleteDevice(device: Device) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Delete Device',
        message: `Are you sure you want to delete ${device.name}?`,
      },
    });

    dialogRef.afterClosed().subscribe((confirmed) => {
      if (confirmed) {
        this.deviceService.delete(device.id).subscribe({
          next: () => {
            this.snackBar.open('Device deleted successfully', 'Close', {
              duration: 3000,
            });
            this.loadDevices();
          },
          error: (err) => this.snackBar.open(err, 'Close', { duration: 3000 }),
        });
      }
    });
  }
}
