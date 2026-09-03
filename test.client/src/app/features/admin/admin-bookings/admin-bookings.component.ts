import { Component, OnInit } from '@angular/core';
import { forkJoin, map, Observable } from 'rxjs';
import { BookingService } from '../../../core/services/booking.service';
import { RoomService } from '../../../core/services/room.service';
import { AdminBooking } from '../../../core/models/booking/AdminBooking';

@Component({
  selector: 'app-admin-bookings',
  templateUrl: './admin-bookings.component.html'
})
export class AdminBookingsComponent implements OnInit {
  bookings$!: Observable<AdminBooking[]>;

  constructor(private bookingService: BookingService, private roomService: RoomService) { }

  ngOnInit(): void {
    this.bookings$ = this.bookingService.getAll();
  }
}
