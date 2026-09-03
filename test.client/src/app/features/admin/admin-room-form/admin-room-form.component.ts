import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { RoomService } from '../../../core/services/room.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-admin-room-form',
  templateUrl: './admin-room-form.component.html'
})
export class AdminRoomFormComponent implements OnInit {
  form: FormGroup;
  errorMessage: string | null = null;
  private roomId: string | null = null;

  get isEditMode(): boolean {
    return this.roomId !== null;
  }

  constructor(
    private fb: FormBuilder,
    private roomService: RoomService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.form = this.fb.group({
      name: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.roomId = this.route.snapshot.paramMap.get('id');
    if (this.roomId) {
      this.roomService.getById(this.roomId).subscribe(room =>
        this.form.patchValue({ name: room.name })
      );
    }
  }

  submit(): void {
    if (this.form.invalid) return;
    this.errorMessage = null;
    const name = this.form.value.name!;

    const request$: Observable<unknown> = this.isEditMode
      ? this.roomService.update(this.roomId!, name)
      : this.roomService.create(name);

    request$.subscribe({
      next: () => this.router.navigate(['/admin/rooms']),
      error: err => this.errorMessage = err.message
    });
  }
}
