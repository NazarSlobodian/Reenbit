import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Room } from '../models/rooms/Room';
import { TimeSlot } from '../models/rooms/TimeSlot';

@Injectable({ providedIn: 'root' })
export class RoomService {
  private baseUrl = `${environment.apiUrl}/rooms`;

  constructor(private http: HttpClient) { }

  getAll(): Observable<Room[]> {
    return this.http.get<Room[]>(this.baseUrl);
  }

  getById(id: string): Observable<Room> {
    return this.http.get<Room>(`${this.baseUrl}/${id}`);
  }

  getSchedule(roomId: string, from: Date, to: Date): Observable<TimeSlot[]> {
    const params = { from: from.toISOString(), to: to.toISOString() };
    return this.http.get<TimeSlot[]>(`${this.baseUrl}/${roomId}/schedule`, { params });
  }

  create(name: string): Observable<{ id: string }> {
    return this.http.post<{ id: string }>(this.baseUrl, { name });
  }

  update(id: string, name: string): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, { name });
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
