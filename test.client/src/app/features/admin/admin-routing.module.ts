import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminRoomListComponent } from './admin-room-list/admin-room-list.component';
import { AdminRoomFormComponent } from './admin-room-form/admin-room-form.component';
import { AdminBookingsComponent } from './admin-bookings/admin-bookings.component';

const routes: Routes = [
  { path: 'rooms', component: AdminRoomListComponent },
  { path: 'rooms/new', component: AdminRoomFormComponent },
  { path: 'rooms/:id/edit', component: AdminRoomFormComponent },
  { path: 'bookings', component: AdminBookingsComponent },
  { path: '', redirectTo: 'rooms', pathMatch: 'full' }
];

@NgModule({ imports: [RouterModule.forChild(routes)], exports: [RouterModule] })
export class AdminRoutingModule { }
