import { Component, OnInit } from '@angular/core';
import { forkJoin, map, Observable } from 'rxjs';
import { BookingService } from '../../../core/services/booking.service';
import { RoomService } from '../../../core/services/room.service';

interface BookingRow { id: string; roomName: string; startTime: string; endTime: string; }

@Component({ selector: 'app-my-bookings', templateUrl: './my-bookings.component.html' })
export class MyBookingsComponent implements OnInit {
  bookings$!: Observable<BookingRow[]>;

  constructor(private bookingService: BookingService, private roomService: RoomService) { }

  ngOnInit(): void {
    this.bookings$ = forkJoin({
      bookings: this.bookingService.getMine(),
      rooms: this.roomService.getAll()
    }).pipe(
      map(({ bookings, rooms }) => bookings.map(b => ({
        id: b.id,
        roomName: rooms.find(r => r.id === b.roomId)?.name ?? b.roomId,
        startTime: b.startTime,
        endTime: b.endTime
      })))
    );
  }
}
