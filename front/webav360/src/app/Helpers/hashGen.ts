import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })

export class DeviceService {
  private readonly STORAGE_KEY = 'device_id';


  async getDeviceHash(): Promise<string> {
    let deviceId = localStorage.getItem(this.STORAGE_KEY);

    if (!deviceId) {
      deviceId = this.generateUUID();
      localStorage.setItem(this.STORAGE_KEY, deviceId);
    }

    return await this.sha256(deviceId);
  }

  private generateUUID(): string {
    if (crypto.randomUUID) {
      return crypto.randomUUID();
    }

    // Fallback simples (caso browser antigo)
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, c => {
      const r = crypto.getRandomValues(new Uint8Array(1))[0] % 16;
      const v = c === 'x' ? r : (r & 0x3) | 0x8;
      return v.toString(16);
    });
  }

  private async sha256(value: string): Promise<string> {
    const data = new TextEncoder().encode(value);
    const hashBuffer = await crypto.subtle.digest('SHA-256', data);
    return Array.from(new Uint8Array(hashBuffer))
      .map(b => b.toString(16).padStart(2, '0'))
      .join('');
  }
}
