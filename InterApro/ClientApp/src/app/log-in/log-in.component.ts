import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { UserService } from '../_services/user.service';
import { Observable } from 'rxjs';
import { User } from '../interfaces';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-log-in',
  templateUrl: './log-in.component.html',
  styleUrls: ['./log-in.component.scss']
})
export class LogInComponent implements OnInit {

  loginForm: FormGroup;
  submitted = false;
  loading: boolean = false;
  user: User[];

  constructor(private _userService: UserService, private formBuilder: FormBuilder, private toastr: ToastrService, private _router: Router) {}

  ngOnInit() {
    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(5)]]
    });
  }

  // convenience getter for easy access to form fields
  get f() { return this.loginForm.controls; }

  onSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.loginForm.invalid) {
      return;
    }

    this.loading = true;

    this._userService.login(this.loginForm).subscribe(results => {
      if (results['success'] == 0) {
        this.toastr.error(results['message'], 'Error', {
          timeOut: 1500,
          progressBar: true
        }).onHidden.subscribe(() => {
          this.loading = false;
        });
      } else {
        this.toastr.success(results['message'], 'Success', {
          timeOut: 1000,
          progressBar: true
        }).onHidden.subscribe(() => {
          localStorage.setItem('currentUser', JSON.stringify(results));
          this._userService.currentUser = localStorage.getItem('currentUser') ? JSON.parse(localStorage.getItem('currentUser')) : '';
          switch (results['rol']) {
            case -1:
              this._router.navigate(['/dashboard/admin']);
              break;
            case 0:
              this._router.navigate(['/dashboard/buyer']);
              break;
            case 1:
              this._router.navigate(['/dashboard/boss']);
              break;
            default:
              this._router.navigate(['/dashboard/financial-approver']);
          }
          this.submitted = false;
          this.loginForm.reset();
          this.loading = false;
          return false;
        });
      }
    }, error => {
        this.toastr.error(error.error['message'], 'Error', {
          timeOut: 1500,
          progressBar: true
        }).onHidden.subscribe(() => {
          this.loading = false;
        });
    });
  }

}
