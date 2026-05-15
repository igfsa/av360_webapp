import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr'
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
  sessaoTurmaCriada$ = new Subject<number>();

  constructor(@Inject(PLATFORM_ID) private platformId: Object) {}

  public connect() {

    if (!isPlatformBrowser(this.platformId)) {
      return;
    }

    this.hub = new HubConnectionBuilder()
      .withUrl(`/hubs/turma`, {
          withCredentials: true,
          accessTokenFactory: () => localStorage.getItem('token')!
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

    this.hub.on('SessaoTurmaCriada', (turmaId: number) => {
      this.sessaoTurmaCriada$.next(turmaId);
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

  public acessarTurma(turmaId: number) {
    return this.hub.invoke('AcessarTurma', turmaId);
  }

}
