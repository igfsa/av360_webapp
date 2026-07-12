import { Injectable, Type } from '@angular/core';
import { Observable, of } from 'rxjs';

import {
    DialogService,
    DynamicDialogConfig
} from 'primeng/dynamicdialog';

export interface ModalOptions {
  header: string;
  width?: string;
  closable?: boolean;
  maximizable?: boolean;
  modal?: boolean;
}

@Injectable()

export class ModalService {

  private readonly defaultConfig: DynamicDialogConfig = {
    modal: true,
    closable: true,
    maximizable: true,
    width: '75vw',
    height: '90vh'
  }

  constructor(
    private dialog: DialogService,
  ) {}

  open<TData, TOut = unknown>(
    component: Type<any>,
    data: TData,
    options?: ModalOptions
  ): Observable<TOut | undefined> {

    const ref = this.dialog.open(component, {
      ...this.defaultConfig,
      ...options,

      data
    });

    if (!ref)
        return of(undefined);

    return ref.onClose;
  }
}
