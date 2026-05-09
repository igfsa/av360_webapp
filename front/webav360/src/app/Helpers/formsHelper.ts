import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })

export class FormsHelper {
  markAllTouched(form: any): void {

    Object.values(form).forEach((field: any) => {
      if (typeof field === 'function' && field().markAsTouched) {
        field().markAsTouched();
      }
    });
  }
}
