import { Injectable } from '@angular/core';
import Swal from 'sweetalert2';

@Injectable({
  providedIn: 'root'
})
export class AlertService {

  success(text: string, title: string = 'Sucesso'): void {
    Swal.fire({
      icon: 'success',
      title,
      html: text
    });
  }


  error(text: string, title: string = 'Erro'): void {
    Swal.fire({
      icon: 'error',
      title,
      html: text
    });
  }


  info(text: string, title: string = 'Info'): void {
    Swal.fire({
      icon: 'info',
      title,
      html: text
    });
  }


  warning(text: string, title: string = 'Atenção'): void {
    Swal.fire({
      icon: 'warning',
      title,
      html: text
    });
  }

  async confirm(
    title: string,
    html: string,
    confirmText: string = 'Confirmar',
    cancelText: string = 'Cancelar'
  ): Promise<boolean> {

    const result = await Swal.fire({
      icon: 'warning',
      title,
      html,
      showDenyButton: true,
      confirmButtonText: confirmText,
      denyButtonText: cancelText
    });

    return result.isConfirmed;
  }

  confirmWarning(
    title: string,
    html: string
  ) {
    return Swal.fire({
      icon: 'warning',
      title,
      html,
      showDenyButton: true,
      confirmButtonText: 'Confirmar',
      denyButtonText: 'Cancelar'
    });
  }

  async confirmDownload(
    title: string,
    text: string
  ): Promise<boolean> {

    const result = await Swal.fire({
      icon: 'warning',
      title,
      text,
      showCancelButton: true,
      confirmButtonText: 'Baixar relatório',
      cancelButtonText: 'Fechar'
    });

    return result.isConfirmed;
  }

  toastSuccess(text: string, title: string = 'Sucesso'): void {
    Swal.mixin({
      toast: true,
      position: 'top-end',
      showConfirmButton: false,
      timer: 3000,
      timerProgressBar: true,
      didOpen: toast => {
        toast.onmouseenter = Swal.stopTimer;
        toast.onmouseleave = Swal.resumeTimer;
      }
    }).fire({
      icon: 'success',
      title,
      html: text
    });
  }


  toastError(text: string, title: string = 'Erro'): void {
    Swal.mixin({
      toast: true,
      position: 'top-end',
      showConfirmButton: false,
      timer: 3000,
      timerProgressBar: true,
      didOpen: toast => {
        toast.onmouseenter = Swal.stopTimer;
        toast.onmouseleave = Swal.resumeTimer;
      }
    }).fire({
      icon: 'error',
      title,
      html: text
    });
  }

}
