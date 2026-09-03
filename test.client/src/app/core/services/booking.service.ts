import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Booking } from '../models/booking/Booking';
import { DeeperBooking } from '../models/booking/DeeperBooking';
import { AdminBooking } from '../models/booking/AdminBooking';

@Injectable({ providedIn: 'root' })
export class BookingService {
  private baseUrl = `${environment.apiUrl}/bookings`;

  constructor(private http: HttpClient) { }

  book(timeSlotId: string): Observable<Booking> {
    return this.http.post<Booking>(`${this.baseUrl}/${timeSlotId}`, null);
  }

  getMine(): Observable<DeeperBooking[]> {
    return this.http.get<DeeperBooking[]>(`${this.baseUrl}/mine`);
  }

  getAll(): Observable<AdminBooking[]> {
    return this.http.get<AdminBooking[]>(this.baseUrl);
  }
}
