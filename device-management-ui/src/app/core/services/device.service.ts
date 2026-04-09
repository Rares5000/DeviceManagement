import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Device, CreateDevice, UpdateDevice } from '../models/device.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class DeviceService {
  private readonly apiUrl = `${environment.apiUrl}/devices`;
  private http = inject(HttpClient);

  getAll(): Observable<Device[]> {
    return this.http.get<Device[]>(this.apiUrl);
  }

  getById(id: number): Observable<Device> {
    return this.http.get<Device>(`${this.apiUrl}/${id}`);
  }

  create(device: CreateDevice): Observable<Device> {
    return this.http.post<Device>(this.apiUrl, device);
  }

  update(id: number, device: UpdateDevice): Observable<Device> {
    return this.http.put<Device>(`${this.apiUrl}/${id}`, device);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  assign(deviceId: number): Observable<Device> {
    return this.http.post<Device>(`${this.apiUrl}/${deviceId}/assign`, {});
  }

  unassign(deviceId: number): Observable<Device> {
    return this.http.post<Device>(`${this.apiUrl}/${deviceId}/unassign`, {});
  }

  generateDescription(deviceId: number): Observable<Device> {
    return this.http.post<Device>(
      `${this.apiUrl}/${deviceId}/generate-description`,
      {},
    );
  }
}
