import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { UserService } from '../_services/user.service';
import { Observable } from 'rxjs';
import { User } from '../interfaces';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-log-in',
  templateUrl: './log-in.component.html',
  styleUrls: ['./log-in.component.scss']
})
export class LogInComponent implements OnInit {

  loginForm: FormGroup;
  submitted = false;
  loading = false;
  user: User[];

  constructor(protected _userService: UserService, private formBuilder: FormBuilder, private toastr: ToastrService) {}

  ngOnInit() {
    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  // convenience getter for easy access to form fields
  get f() { return this.loginForm.controls; }

  onSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.loginForm.invalid) {
      //this.loading = false;
      return;
    }

    this.loading = true;
    this.toastr.success('Hello world!', 'Toastr fun!');
    this.toastr.error('everything is broken', 'Major Error', {
      timeOut: 3000
    });

    this._userService.loginUser(this.loginForm).subscribe(results => {
      console.log('results', results);
    }, error => console.error('error', error));

    // display form values on success
    //alert('INFO:\n\n' + JSON.stringify(this.loginForm.value, null, 4));
  }

}
