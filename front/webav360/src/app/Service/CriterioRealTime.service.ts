import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr'
import { baseURL } from '../../main.server';
import { isPlatformBrowser } from '@angular/common';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CriterioRealTime {

  private hub!: HubConnection;

  criterioAtualizado$ = new Subject<number>();

  constructor(@Inject(PLATFORM_ID) private platformId: Object) {}

  connect() {

    if (!isPlatformBrowser(this.platformId)) {
      return;
    }

    this.hub = new HubConnectionBuilder()
      .withUrl(`${baseURL}hubs/criterio`, {
         withCredentials: true
       })
      .withAutomaticReconnect()
      .build();

    this.hub.on('CriterioAtualizado', (criterioId: number) => {
      this.criterioAtualizado$.next(criterioId);
    });

    return this.hub.start()
        .catch(err => console.error('SignalR error:', err));
  }

  public acessarCriterio(criterioId: number) {
    return this.hub.invoke('AcessarCriterio', criterioId);
  }

}
