import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RoomListComponent } from './room-list/room-list.component';
import { RoomScheduleComponent } from './room-schedule/room-schedule.component';

const routes: Routes = [
  { path: '', component: RoomListComponent },
  { path: ':id', component: RoomScheduleComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RoomsRoutingModule { }
