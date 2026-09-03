// admin-room-list.component.ts
import { Component, OnInit } from '@angular/core';
import { Room } from '../../../core/models/rooms/Room';
import { RoomService } from '../../../core/services/room.service';

@Component({
  selector: 'app-admin-room-list',
  templateUrl: './admin-room-list.component.html'
})
export class AdminRoomListComponent implements OnInit {
  rooms: Room[] = [];
  errorMessage: string | null = null;

  constructor(private roomService: RoomService) { }

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.roomService.getAll().subscribe(rooms => this.rooms = rooms);
  }

  delete(room: Room): void {
    if (!confirm(`Delete "${room.name}"? Existing bookings for it are kept, but it stops being bookable.`)) return;

    this.errorMessage = null;
    this.roomService.delete(room.id).subscribe({
      next: () => this.load(),
      error: err => this.errorMessage = err.message
    });
  }
}
