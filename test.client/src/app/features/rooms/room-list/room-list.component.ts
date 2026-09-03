import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Room } from '../../../core/models/rooms/Room';
import { RoomService } from '../../../core/services/room.service';

@Component({
  selector: 'app-room-list',
  templateUrl: './room-list.component.html'
})
export class RoomListComponent implements OnInit {
  rooms$!: Observable<Room[]>;

  constructor(private roomService: RoomService) { }

  ngOnInit(): void {
    this.rooms$ = this.roomService.getAll();
  }
}
