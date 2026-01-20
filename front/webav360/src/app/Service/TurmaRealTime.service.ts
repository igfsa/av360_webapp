import { Injectable, Signal } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr'

@Injectable({
  providedIn: 'root'
})
export class TurmaRealTime {

  private hub!: HubConnection;

  connect() {
    this.hub = new HubConnectionBuilder()
      .withUrl('/hubs/turma')
      .withAutomaticReconnect()
      .build();

    this.hub.start();
  }

  onTurmaAtualizada(callback: (id: number) => void) {
    this.hub.on('TurmaAtualizada', callback);
  }

}
