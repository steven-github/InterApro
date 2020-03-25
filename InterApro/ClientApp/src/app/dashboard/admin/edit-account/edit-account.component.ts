import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { MustMatch } from '../../../_helpers/must-match.validator';
import { UserService } from '../../../_services/user.service';
import { Observable } from 'rxjs';
import { User } from '../../../interfaces';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-admin-edit-account',
  templateUrl: './edit-account.component.html',
  styleUrls: ['./edit-account.component.scss']
})
export class AdminEditAccountComponent implements OnInit {

  @Input() isOpen: number;
  @Input() userId: number;
  // @Input() user: User;
  @Output() closeModal = new EventEmitter<number>();

  editAccountForm: FormGroup;
  submitted = false;
  loading = false;
  user: User[];

  constructor(private _userService: UserService, private formBuilder: FormBuilder, private toastr: ToastrService) {
    // this.getUser();
    console.log('constructor');
  }

  ngOnInit() {
    this.editAccountForm = this.formBuilder.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      username: ['', Validators.required],
      password: [],
      confirmPassword: [],
      status: [],
      rol: ['', Validators.required],
    }, {
      validator: MustMatch('password', 'confirmPassword')
    });
    console.log('ngOnInit');
  }

  ngAfterViewInit() {
    console.log('ngAfterViewInit');
  }

  ngAfterViewChecked() {
    // console.log('ngAfterViewChecked');
  }

  ngOnChanges() {
    console.log('ngOnChanges');
    if (this.isOpen) {
      setTimeout (() => {
        console.log("Hello from setTimeout");
        this.getUser();
      }, 0);  
    }
  }

  ngDoCheck() {
    console.log('ngDoCheck');
  }

  viewToModelUpdate(newValue: any): void {
    console.log('viewToModelUpdate');
  }

  doSomething(event) {
    console.log('doSomething', event); // logs model value
  }

  getUser() {
    this._userService.getUser(this.userId).subscribe(results => {
      if (results['success'] == 1) {
        console.log('getUser', results);
        this.f.firstName.setValue(results['firstname']);
        this.f.lastName.setValue(results['lastname']);
        this.f.email.setValue(results['email']);
        this.f.username.setValue(results['username']);
        this.f.status.setValue(results['status']);
        this.f.rol.setValue(results['rol']);
      }
    }, error => {
      console.log('error', error);
    });
  }

  // convenience getter for easy access to form fields
  get f() { return this.editAccountForm.controls; }

  onSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.editAccountForm.invalid) {
      return;
    }

    this.loading = true;

    this._userService.edit(this.userId, this.editAccountForm).subscribe(results => {
      console.log('results', results);
      if (results['success'] == 0) {
        this.toastr.error(results['message'], 'Error', {
          timeOut: 1000,
          progressBar: true
        }).onHidden.subscribe(() => {
          this.loading = false;
        });
      } else {
        this.toastr.success(results['message'], 'Success', {
          timeOut: 1000,
          progressBar: true
        }).onHidden.subscribe(() => {
          this.submitted = false;
          this.editAccountForm.reset();
          this.loading = false;
          this.closeModal.emit(1);
          return false;
        });
      }
    }, error => console.error('error', error));
  }

}

