import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AuthService } from './auth.service';

export interface TimeSlotStatusChangedEvent {
  timeSlotId: string;
  newStatus: number;
}

@Injectable({ providedIn: 'root' })
export class SignalRService {
  private connection: signalR.HubConnection | null = null;
  private statusChanged = new Subject<TimeSlotStatusChangedEvent>();
  timeSlotStatusChanged$ = this.statusChanged.asObservable();

  constructor(private authService: AuthService) { }

  async connect(): Promise<void> {
    if (this.connection) return;

    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(environment.signalRUrl, {
        accessTokenFactory: () => this.authService.token ?? ''
      })
      .withAutomaticReconnect()
      .build();

    this.connection.on('TimeSlotStatusChanged', (timeSlotId: string, newStatus: number) => {
      this.statusChanged.next({ timeSlotId, newStatus });
    });

    await this.connection.start();
  }

  async joinRoomGroup(roomId: string): Promise<void> {
    await this.connection?.invoke('JoinRoomGroup', roomId);
  }

  async leaveRoomGroup(roomId: string): Promise<void> {
    await this.connection?.invoke('LeaveRoomGroup', roomId);
  }
}
