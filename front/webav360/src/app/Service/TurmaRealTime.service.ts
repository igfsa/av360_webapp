import { Inject, Injectable, PLATFORM_ID, Signal } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr'
import { baseURL } from '../../main.server';
import { isPlatformBrowser } from '@angular/common';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TurmaRealTime {

  private hub!: HubConnection;

  turmaAtualizada$ = new Subject<number>();
  alunoTurmaAtualizada$ = new Subject<number>();
  criterioTurmaAtualizada$ = new Subject<number>();

  constructor(@Inject(PLATFORM_ID) private platformId: Object) {}

  connect() {

    if (!isPlatformBrowser(this.platformId)) {
      return;
    }

    this.hub = new HubConnectionBuilder()
      .withUrl(`${baseURL}hubs/turma`, {
         withCredentials: true
       })
      .withAutomaticReconnect()
      .build();

    this.hub.on('TurmaAtualizada', (turmaId: number) => {
      this.turmaAtualizada$.next(turmaId);
    });

    this.hub.on('AlunoTurmaAtualizada', (turmaId: number) => {
      this.alunoTurmaAtualizada$.next(turmaId);
    });

    this.hub.on('CriterioTurmaAtualizada', (turmaId: number) => {
      this.criterioTurmaAtualizada$.next(turmaId);
    });

    return this.hub.start()
        .catch(err => console.error('SignalR error:', err));
  }

  public acessarTurma(turmaId: number) {
    return this.hub.invoke('AcessarTurma', turmaId);
  }

}
