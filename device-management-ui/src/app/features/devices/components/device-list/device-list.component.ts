import { Component, inject, signal, computed } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatTooltipModule } from '@angular/material/tooltip';
import { DeviceService } from '../../../../core/services/device.service';
import { AuthService } from '../../../../core/services/auth.service';
import { Device } from '../../../../core/models/device.model';
import { ConfirmDialogComponent } from '../../../../shared/components/confirm-dialog/confirm-dialog.component';
import { LoadingSpinnerComponent } from '../../../../shared/components/loading-spinner/loading-spinner.component';
import { SearchBarComponent } from '../../../../shared/components/search-bar/search-bar.component';

@Component({
  selector: 'app-device-list',
  standalone: true,
  imports: [
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatSnackBarModule,
    MatDialogModule,
    MatTooltipModule,
    LoadingSpinnerComponent,
    SearchBarComponent,
    RouterLink,
  ],
  templateUrl: './device-list.component.html',
  styleUrls: ['./device-list.component.scss'],
})
export class DeviceListComponent {
  private deviceService = inject(DeviceService);
  private authService = inject(AuthService);
  private router = inject(Router);
  private dialog = inject(MatDialog);
  private snackBar = inject(MatSnackBar);

  isLoading = signal(false);
  isSearching = signal(false);
  searchQuery = signal('');
  devices = signal<Device[]>([]);
  currentUserId = this.authService.currentUserId;

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

  onSearch(query: string) {
    this.searchQuery.set(query);
    this.isSearching.set(true);
    this.deviceService.search(query).subscribe({
      next: (devices) => {
        this.devices.set(devices);
        this.isSearching.set(false);
      },
      error: (err) => {
        this.snackBar.open(err, 'Close', { duration: 3000 });
        this.isSearching.set(false);
      },
    });
  }

  onSearchCleared() {
    this.searchQuery.set('');
    this.loadDevices();
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

  assignDevice(device: Device) {
    this.deviceService.assign(device.id).subscribe({
      next: () => {
        this.snackBar.open(`${device.name} assigned to you`, 'Close', {
          duration: 3000,
        });
        this.loadDevices();
      },
      error: (err) => this.snackBar.open(err, 'Close', { duration: 3000 }),
    });
  }

  unassignDevice(device: Device) {
    this.deviceService.unassign(device.id).subscribe({
      next: () => {
        this.snackBar.open(`${device.name} unassigned`, 'Close', {
          duration: 3000,
        });
        this.loadDevices();
      },
      error: (err) => this.snackBar.open(err, 'Close', { duration: 3000 }),
    });
  }

  isAssignedToCurrentUser(device: Device): boolean {
    return device.assignedUserId === this.currentUserId();
  }
}
