export enum DeviceType {
  Phone = 'Phone',
  Tablet = 'Tablet',
}

export interface Device {
  id: number;
  name: string;
  manufacturer: string;
  type: DeviceType;
  operatingSystem: string;
  osVersion: string;
  processor: string;
  ramAmount: number;
  serialNumber: string;
  description: string;
  assignedUserId?: number;
  assignedUserName?: string;
  assignedUserLocation?: string;
}

export interface CreateDevice {
  name: string;
  manufacturer: string;
  type: DeviceType;
  operatingSystem: string;
  osVersion: string;
  processor: string;
  ramAmount: number;
  serialNumber: string;
  description: string;
}

export interface UpdateDevice {
  name: string;
  manufacturer: string;
  type: DeviceType;
  operatingSystem: string;
  osVersion: string;
  processor: string;
  ramAmount: number;
  serialNumber: string;
  description: string;
}
