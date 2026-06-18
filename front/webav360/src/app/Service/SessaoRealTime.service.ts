import { isPlatformBrowser } from '@angular/common';
import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { baseURL } from '../../main.server';

@Injectable({
  providedIn: 'root'
})
export class SessaoRealTime {
  private hub!: HubConnection;

  sessaoAtualizada$ = new Subject<number>();
  sessaoFinalizada$ = new Subject<number>();

  constructor(@Inject(PLATFORM_ID) private platformId: Object) {}

  public connect() {

    if (!isPlatformBrowser(this.platformId)) {
      return;
    }

    this.hub = new HubConnectionBuilder()
      .withUrl(`${baseURL}/hubs/sessao`, {
          withCredentials: true,
          accessTokenFactory: () => localStorage.getItem('token')!
      })
      .withAutomaticReconnect()
      .build();

    this.hub.on('SessaoAtualizada', (sessaoId: number) => {
      this.sessaoAtualizada$.next(sessaoId);
    });

    this.hub.on('NovaAvaliacao', (sessaoId: number) => {
      this.sessaoAtualizada$.next(sessaoId);
    });

    this.hub.on('SessaoFinalizada', (sessaoId: number) => {
      this.sessaoFinalizada$.next(sessaoId);
    });

    return this.hub.start()
        .catch(err => console.error('SignalR error:', err));
  }

  public disconnect() {

    if (this.hub) {

      this.hub.stop()
        .catch(err =>
          console.error('Erro ao desconectar SignalR:', err)
        );

      this.hub = undefined!;
    }
  }

  public acessarSessao(sessaoId: number) {
    return this.hub.invoke('AcessarSessao', sessaoId);
  }

}
