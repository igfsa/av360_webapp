import { isPlatformBrowser } from '@angular/common';
import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { API_URL } from '../app.config';

@Injectable({
  providedIn: 'root'
})
export class ProfessorRealTime {

  private hub!: HubConnection;

  professorAtualizado$ = new Subject<number>();

  constructor(@Inject(PLATFORM_ID) private platformId: Object, @Inject(API_URL) public readonly baseURL: string) {}

  connect() {

    if (!isPlatformBrowser(this.platformId)) {
      return;
    }

    this.hub = new HubConnectionBuilder()
      .withUrl(`${this.baseURL}/hubs/professor`, {
          withCredentials: true,
          accessTokenFactory: () => localStorage.getItem('token')!
       })
      .withAutomaticReconnect()
      .build();

    this.hub.on('ProfessorAtualizado', (professorId: number) => {
      this.professorAtualizado$.next(professorId);
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

  public acessarProfessor(professorId: number) {
    return this.hub.invoke('AcessarProfessor', professorId);
  }

}
