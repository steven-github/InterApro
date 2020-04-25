import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { User } from 'src/app/interfaces';
import { UserService } from 'src/app/_services/user.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-buyer-create-request',
  templateUrl: './create-request.component.html',
  styleUrls: ['./create-request.component.scss']
})
export class CreateRequestComponent implements OnInit {

  // @Input() test: boolean;
  // @Output() testChange = new EventEmitter<boolean>();
  // @Input() closeModal: any;
  @Output() closeModal = new EventEmitter<number>();

  createRequestForm: FormGroup;
  submitted = false;
  loading = false;
  //users: Observable<User[]>;
  users: User[];
  boss: User[] = new Array();

  constructor(private _userService: UserService, private formBuilder: FormBuilder, private toastr: ToastrService) {
    this.getBoss();
  }

  ngOnInit() {
    this.createRequestForm = this.formBuilder.group({
      userId: ['', Validators.required],
      bossId: ['', Validators.required],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      username: ['', Validators.required],
      assigneeId: ['', Validators.required],
      assigneeName: ['', Validators.required],
      price: ['', Validators.required],
      description: ['', Validators.required]
    });
  }

  // convenience getter for easy access to form fields
  get f() { return this.createRequestForm.controls; }

  onSubmit() {
    this.submitted = true;
    // this.testChange.emit(!this.test);

    // stop here if form is invalid
    if (this.createRequestForm.invalid) {
      return;
    }

    this.loading = true;

    this._userService.createRequest(this.createRequestForm).subscribe(results => {
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
          this.createRequestForm.reset();
          this.loading = false;
          this.closeModal.emit(0);
          return false;
        });
      }
    }, error => console.error('error', error));
  }

  getBoss(): void {
    this.loading = true;
    this._userService.getBoss(this._userService.currentUserValue.bossId).subscribe(results => {
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
          this.boss = results;
          this.createRequestForm.get('userId').setValue(this._userService.currentUserValue.userId);
          this.createRequestForm.get('bossId').setValue(this._userService.currentUserValue.bossId);
          this.createRequestForm.get('firstName').setValue(this._userService.currentUserValue.firstName);
          this.createRequestForm.get('lastName').setValue(this._userService.currentUserValue.lastName);
          this.createRequestForm.get('email').setValue(this._userService.currentUserValue.email);
          this.createRequestForm.get('username').setValue(this._userService.currentUserValue.username);
          this.createRequestForm.get('assigneeId').setValue(this._userService.currentUserValue.bossId);
          this.createRequestForm.get('assigneeName').setValue(results['firstName'] + ' ' + results['lastName']);
          this.loading = false;
        });
      }
    }, error => {
      this.toastr.error(error, 'Attention', {
        timeOut: 1500,
        progressBar: true
      }).onHidden.subscribe(() => {
        this.loading = false;
      });
    });
  }

}
