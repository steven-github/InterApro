import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { User } from 'src/app/interfaces';
import { UserService } from 'src/app/_services/user.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-buyer-edit-request',
  templateUrl: './edit-request.component.html',
  styleUrls: ['./edit-request.component.scss']
})
export class EditRequestComponent implements OnInit {

  @Input() isOpen: number;
  @Input() requestId: number;
  @Output() closeModal = new EventEmitter<number>();

  editRequestForm: FormGroup;
  submitted = false;
  loading = false;
  blockPasswordFields = true;
  user: User[];

  constructor(private _userService: UserService, private formBuilder: FormBuilder, private toastr: ToastrService) {
    // this.getUser();
  }

  ngOnInit() {
    this.editRequestForm = this.formBuilder.group({
      price: ['', Validators.required],
      description: ['', Validators.required]
    });
  }

  ngOnChanges() {
    console.log('ngOnChanges');
    if (this.isOpen) {
      setTimeout(() => {
        this.getRequest();
      }, 0);
    }
  }

  getRequest() {
    this._userService.getRequest(this.requestId).subscribe(results => {
      if (results['success'] == 1) {
        console.log('getRequest', results);
        this.f.price.setValue(results['price']);
        this.f.description.setValue(results['description']);
      }
    }, error => {
      console.log('error', error);
    });
  }

  // convenience getter for easy access to form fields
  get f() { return this.editRequestForm.controls; }

  onSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.editRequestForm.invalid) {
      return;
    }

    this.loading = true;

    console.log('this.requestId', this.requestId);

    this._userService.editRequest(this.requestId, this.editRequestForm).subscribe(results => {
      console.log('editRequest', results);
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
          this.editRequestForm.reset();
          this.loading = false;
          this.closeModal.emit(1);
          return false;
        });
      }
    }, error => console.error('error', error));
  }

}
