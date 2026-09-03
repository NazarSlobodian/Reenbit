import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Booking } from '../models/booking/Booking';

@Injectable({ providedIn: 'root' })
export class BookingService {
  private baseUrl = `${environment.apiUrl}/bookings`;

  constructor(private http: HttpClient) { }

  book(timeSlotId: string): Observable<Booking> {
    return this.http.post<Booking>(`${this.baseUrl}/${timeSlotId}`, null);
  }

  getMine(): Observable<Booking[]> {
    return this.http.get<Booking[]>(`${this.baseUrl}/mine`);
  }

  getAll(): Observable<Booking[]> {
    return this.http.get<Booking[]>(this.baseUrl);
  }
}
