import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RoomsRoutingModule } from './rooms-routing.module';
import { RoomListComponent } from './room-list/room-list.component';
import { RoomScheduleComponent } from './room-schedule/room-schedule.component';

@NgModule({
  declarations: [RoomListComponent, RoomScheduleComponent],
  imports: [CommonModule, RoomsRoutingModule]
})
export class RoomsModule { }
