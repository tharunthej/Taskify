// edit-profile-dialog.component.ts
import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { CommonModule } from '@angular/common';
import { User } from '../../../models/user.model';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInput, MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-edit-profile-dialog',
  standalone: true,
  templateUrl: './edit-profile-dialog.component.html',
  styleUrls: ['./edit-profile-dialog.component.scss'],
  imports: [
    CommonModule,
    MatDialogModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    ReactiveFormsModule
  ]
})
export class EditProfileDialogComponent {
  editForm: FormGroup;
  hideCurrentPassword = true;
  hideNewPassword = true;
  hideConfirmPassword = true;

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<EditProfileDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { user: User }
  ) {
    this.editForm = this.fb.group({
      username: [data.user.username, Validators.required],
      email: [data.user.email, [Validators.required, Validators.email]],
      currentPassword: [''],
      newPassword: ['', [Validators.minLength(6)]],
      confirmPassword: ['']
    }, { validators: this.passwordMatchValidator });
  }

  passwordMatchValidator(form: FormGroup) {
    return form.get('newPassword')?.value === form.get('confirmPassword')?.value
      ? null : { passwordMismatch: true };
  }

  onSubmit(): void {
    if (this.editForm.valid) {
      const formData = {
        username: this.editForm.value.username,
        email: this.editForm.value.email,
        currentPassword: this.editForm.value.currentPassword,
        newPassword: this.editForm.value.newPassword
      };
      this.dialogRef.close(formData);
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}