import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { BookingService } from '../../../core/services/booking.service';
import { DeeperBooking } from '../../../core/models/booking/DeeperBooking';

@Component({
  selector: 'app-my-bookings',
  templateUrl: './my-bookings.component.html'
})
export class MyBookingsComponent implements OnInit {
  bookings$!: Observable<DeeperBooking[]>;

  constructor(private bookingService: BookingService) { }

  ngOnInit(): void {
    this.bookings$ = this.bookingService.getMine();
  }
}
