import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { MustMatch } from '../../../_helpers/must-match.validator';
import { UserService } from '../../../_services/user.service';
import { Observable } from 'rxjs';
import { User } from '../../../interfaces';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-admin-create-account',
  templateUrl: './create-account.component.html',
  styleUrls: ['./create-account.component.scss']
})
export class AdminCreateAccountComponent implements OnInit {

  // @Input() test: boolean;
  // @Output() testChange = new EventEmitter<boolean>();
  // @Input() closeModal: any;
  @Output() closeModal = new EventEmitter<number>();

  createAccountForm: FormGroup;
  submitted = false;
  loading = false;
  //users: Observable<User[]>;
  users: User[];
  bosses: User[] = new Array();

  constructor(private _userService: UserService, private formBuilder: FormBuilder, private toastr: ToastrService) {
    this.getBosses();
  }

  ngOnInit() {
    this.createAccountForm = this.formBuilder.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      username: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(5)]],
      confirmPassword: ['', Validators.required],
      status: [null],
      bossId: [null],
      rol: ['', Validators.required],
    }, {
      validator: MustMatch('password', 'confirmPassword')
    });

    this.createAccountForm.get('rol').valueChanges
      .subscribe(rol => {
        console.log('rol', rol);
        if (rol === '0') {
          this.createAccountForm.get('bossId').setValidators([Validators.required]);
        } else {
          this.createAccountForm.get('bossId').setValidators(null);
        }

        // if (userCategory === 'employee') {
        //   institutionControl.setValidators(null);
        //   companyControl.setValidators([Validators.required]);
        //   salaryControl.setValidators([Validators.required]);
        // }

        // institutionControl.updateValueAndValidity();
        // companyControl.updateValueAndValidity();
        // salaryControl.updateValueAndValidity();
      });
  }

  // convenience getter for easy access to form fields
  get f() { return this.createAccountForm.controls; }

  onSubmit() {
    this.submitted = true;
    // this.testChange.emit(!this.test);

    // stop here if form is invalid
    if (this.createAccountForm.invalid) {
      return;
    }

    this.loading = true;

    this._userService.createUser(this.createAccountForm).subscribe(results => {
      console.log('results', results);
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
          this.createAccountForm.reset();
          this.loading = false;
          this.closeModal.emit(0);
          return false;
        });
      }
    }, error => console.error('error', error));
  }

  getBosses(): void {
    this.loading = true;
    this._userService.getBosses().subscribe(results => {
      if (results['success'] == 0) {
        this.toastr.error(results['message'], 'Attention', {
          timeOut: 1500,
          progressBar: true
        }).onHidden.subscribe(() => {
          this.loading = false;
        });
      } else {
        this.toastr.success(results['message'], 'Success', {
          timeOut: 500,
          progressBar: true
        }).onHidden.subscribe(() => {
          this.bosses = results['bosses'];
          this.loading = false;
        });
      }
    }, error => {
      console.log('asd', this.bosses.length);
      this.toastr.warning(error, 'Not Bosses Found', {
        timeOut: 3000,
        progressBar: false,
        // closeButton: true
      }).onHidden.subscribe(() => {
        this.loading = false;
      });
    });
  }

}
