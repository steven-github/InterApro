import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MustMatch } from 'src/app/_helpers/must-match.validator';
import { UserService } from 'src/app/_services/user.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-buyer-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileBuyerComponent implements OnInit {

  editAccountBuyerForm: FormGroup;
  submitted = false;
  loading = false;

  constructor(private _userService: UserService, private formBuilder: FormBuilder, private toastr: ToastrService) {
    this.getUser();
  }

  ngOnInit() {
    this.editAccountBuyerForm = this.formBuilder.group({
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

    // this.editAccountBuyerForm.get('password').valueChanges
    //   .subscribe(password => {
    //     console.log('password', password);
    //     if (!!password) {
    //       this.editAccountBuyerForm.get('password').setValidators([Validators.required]);
    //       this.editAccountBuyerForm.get('confirmPassword').setValidators([Validators.required]);
    //     } else {
    //       this.editAccountBuyerForm.get('password').setValidators(null);
    //       this.editAccountBuyerForm.get('confirmPassword').setValidators(null);
    //     }
    //     this.editAccountBuyerForm.get('password').updateValueAndValidity();
    //     this.editAccountBuyerForm.get('confirmPassword').updateValueAndValidity();
    //   });
  }

  // convenience getter for easy access to form fields
  get f() { return this.editAccountBuyerForm.controls; }

  onSubmit() {
    this.submitted = true;
    console.log('asd', this.editAccountBuyerForm);

    // stop here if form is invalid
    if (this.editAccountBuyerForm.invalid) {
      return;
    }

    this.loading = true;

    this._userService.editUser(this._userService.currentUserValue.userId, this.editAccountBuyerForm).subscribe(results => {
      if (results['success'] == 0) {
        this.toastr.error(results['message'], 'Attention', {
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
          this.loading = false;
          return false;
        });
      }
    }, error => console.error('error', error));
  }

  getUser() {
    this._userService.getUser(this._userService.currentUserValue.userId).subscribe(results => {
      console.log('getUser', results);
      if (results['success'] == 1) {
        this.f.firstName.setValue(results['firstName']);
        this.f.lastName.setValue(results['lastName']);
        this.f.email.setValue(results['email']);
        this.f.username.setValue(results['username']);
        this.f.password.setValue(results['password']);
        this.f.confirmPassword.setValue(results['confirmPassword']);
        this.f.status.setValue(results['status']);
        this.f.rol.setValue(results['rol']);
      }
    }, error => {
      console.log('error', error);
    });
  }

}
