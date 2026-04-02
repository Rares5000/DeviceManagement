import { Component, OnInit, inject, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { DeviceService } from '../../../../core/services/device.service';
import { Device } from '../../../../core/models/device.model';
import { LoadingSpinnerComponent } from '../../../../shared/components/loading-spinner/loading-spinner.component';

@Component({
  selector: 'app-device-detail',
  standalone: true,
  imports: [
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    MatSnackBarModule,
    LoadingSpinnerComponent,
  ],
  templateUrl: './device-detail.component.html',
  styleUrls: ['./device-detail.component.scss'],
})
export class DeviceDetailComponent implements OnInit {
  private deviceService = inject(DeviceService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private snackBar = inject(MatSnackBar);

  device = signal<Device | undefined>(undefined);
  isLoading = signal(false);

  ngOnInit() {
    const id = this.route.snapshot.params['id'];
    this.loadDevice(+id);
  }

  loadDevice(id: number) {
    this.isLoading.set(true);
    this.deviceService.getById(id).subscribe({
      next: (device) => {
        this.device.set(device);
        this.isLoading.set(false);
      },
      error: (err) => {
        this.snackBar.open(err, 'Close', { duration: 3000 });
        this.isLoading.set(false);
        this.router.navigate(['/devices']);
      },
    });
  }

  editDevice() {
    this.router.navigate(['/devices', this.device()?.id, 'edit']);
  }

  goBack() {
    this.router.navigate(['/devices']);
  }
}
