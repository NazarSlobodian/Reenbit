import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BookingsRoutingModule } from './bookings-routing.module';
import { MyBookingsComponent } from './my-bookings/my-bookings.component';


@NgModule({
  declarations: [
    MyBookingsComponent
  ],
  imports: [
    CommonModule,
    BookingsRoutingModule
  ]
})
export class BookingsModule { }
