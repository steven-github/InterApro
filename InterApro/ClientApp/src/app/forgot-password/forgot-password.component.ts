import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss']
})
export class ForgotPasswordComponent implements OnInit {

  forgotPasswordForm: FormGroup;
  submitted = false;

  constructor(private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.forgotPasswordForm = this.formBuilder.group({
      username: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]]
    });
  }

  // convenience getter for easy access to form fields
  get f() { return this.forgotPasswordForm.controls; }

  onSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.forgotPasswordForm.invalid
      && this.forgotPasswordForm.controls.email.invalid
      && this.forgotPasswordForm.controls.username.invalid) {
      return;
    } else {
      if (this.forgotPasswordForm.controls.username.valid) {
        this.forgotPasswordForm.controls['email'].setErrors(null);
      }
      if (this.forgotPasswordForm.controls.email.valid) {
        this.forgotPasswordForm.controls['username'].setErrors(null);
      }
    }

    // display form values on success
    // alert('SUCCESS!! :-)\n\n' + JSON.stringify(this.createAccountForm.value, null, 4));
  }

}
