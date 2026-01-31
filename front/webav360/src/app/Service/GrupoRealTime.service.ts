import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr'
import { baseURL } from '../../main.server';
import { isPlatformBrowser } from '@angular/common';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GrupoRealTime {

  private hub!: HubConnection;

  grupoAtualizado$ = new Subject<number>();

  constructor(@Inject(PLATFORM_ID) private platformId: Object) {}

  connect() {

    if (!isPlatformBrowser(this.platformId)) {
      return;
    }

    this.hub = new HubConnectionBuilder()
      .withUrl(`${baseURL}hubs/grupo`, {
         withCredentials: true
       })
      .withAutomaticReconnect()
      .build();

    this.hub.on('GrupoAtualizado', (grupoId: number) => {
      this.grupoAtualizado$.next(grupoId);
    });

    return this.hub.start()
        .catch(err => console.error('SignalR error:', err));
  }

  public acessarGrupo(grupoId: number) {
    return this.hub.invoke('AcessarGrupo', grupoId);
  }

}
