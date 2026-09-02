import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html'
})
export class RegisterComponent {
  form: FormGroup;
  errorMessage: string | null = null;

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
      fullName: ['', Validators.required]
    });
  }

  submit(): void {
    if (this.form.invalid) return;
    this.errorMessage = null;

    this.authService.register(this.form.getRawValue() as any).subscribe({
      next: () => this.router.navigate(['/rooms']),
      error: err => this.errorMessage = err.message
    });
  }
}
