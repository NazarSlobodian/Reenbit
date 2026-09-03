import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { AdminRoutingModule } from './admin-routing.module';
import { AdminRoomListComponent } from './admin-room-list/admin-room-list.component';
import { AdminRoomFormComponent } from './admin-room-form/admin-room-form.component';
import { AdminBookingsComponent } from './admin-bookings/admin-bookings.component';


@NgModule({
  declarations: [
    AdminRoomListComponent,
    AdminRoomFormComponent,
    AdminBookingsComponent
  ],
  imports: [
    CommonModule,
    AdminRoutingModule,
    ReactiveFormsModule
  ]
})
export class AdminModule { }
