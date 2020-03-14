import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { MustMatch } from '../_helpers/must-match.validator';
import { UserService } from '../_services/user.service';
import { Observable } from 'rxjs';
import { User } from '../interfaces';

@Component({
  selector: 'app-create-account',
  templateUrl: './create-account.component.html',
  styleUrls: ['./create-account.component.scss']
})
export class CreateAccountComponent implements OnInit {

  createAccountForm: FormGroup;
  submitted = false;
  users: Observable<User[]>;

  constructor(protected _userService: UserService, private formBuilder: FormBuilder) {
    this.getUsers();
  }

  ngOnInit() {
    this.createAccountForm = this.formBuilder.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      username: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required],
      rol: ['', Validators.required],
    }, {
      validator: MustMatch('password', 'confirmPassword')
    });
  }

  // convenience getter for easy access to form fields
  get f() { return this.createAccountForm.controls; }

  onSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.createAccountForm.invalid) {
      return;
    }

    this._userService.saveUser(this.createAccountForm);

    setTimeout(() => {
      this.getUsers();
    }, 200);

    // display form values on success
    //alert('INFO:\n\n' + JSON.stringify(this.createAccountForm.value, null, 4));
  }

  getUsers() {
    this.users = this._userService.getUsers();
    console.log('this.users', this.users);
  }

}
