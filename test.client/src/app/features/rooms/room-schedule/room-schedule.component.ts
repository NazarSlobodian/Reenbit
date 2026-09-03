import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { TimeSlot } from '../../../core/models/rooms/TimeSlot';
import { TimeSlotStatus } from '../../../core/models/rooms/TimeSlotStatus';
import { RoomService } from '../../../core/services/room.service';
import { BookingService } from '../../../core/services/booking.service';

@Component({
  selector: 'app-room-schedule',
  templateUrl: './room-schedule.component.html'
})
export class RoomScheduleComponent implements OnInit, OnDestroy {
  TimeSlotStatus = TimeSlotStatus;
  roomId!: string;
  slots: TimeSlot[] = [];
  errorMessage: string | null = null;
  private signalRSub?: Subscription;

  constructor(
    private route: ActivatedRoute,
    private roomService: RoomService,
    private bookingService: BookingService
  ) { }

  async ngOnInit(): Promise<void> {
    this.roomId = this.route.snapshot.paramMap.get('id')!;
    this.loadSchedule();
  }

  async ngOnDestroy(): Promise<void> {
  }

  private loadSchedule(): void {
    const from = new Date();
    const to = new Date();
    to.setDate(to.getDate() + 14);

    this.roomService.getSchedule(this.roomId, from, to)
      .subscribe(slots => this.slots = slots);
  }

  book(slot: TimeSlot): void {
    this.errorMessage = null;
    this.bookingService.book(slot.id).subscribe({
      error: err => this.errorMessage = err.message
    });
  }
}
