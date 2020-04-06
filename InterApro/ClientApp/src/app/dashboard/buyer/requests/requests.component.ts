import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { UserService } from 'src/app/_services/user.service';
import { ToastrService } from 'ngx-toastr';
import { User } from 'src/app/interfaces';
declare var $: any;

@Component({
  selector: 'app-buyer-requests',
  templateUrl: './requests.component.html',
  styleUrls: ['./requests.component.scss']
})
export class RequestsBuyerComponent implements OnInit {

  createRequestForm: FormGroup;
  boss: User[];
  requests: Request[] = new Array();
  submitted = false;
  loading = false;
  assigneeName: string;
  isOpen: boolean = false;
  requestId: number;
  // @ViewChild('closeModal') closeModal: ElementRef;

  constructor(private _userService: UserService, private formBuilder: FormBuilder, private toastr: ToastrService) {
    console.log('_userService.currentUserValue', _userService.currentUserValue);
    this.getRequests();
  }

  ngOnInit(): void {
    $('[data-toggle="tooltip"]').tooltip();
  }

  getRequests(): void {
    this.loading = true;
    this._userService.getRequests().subscribe(results => {
      console.log('getRequests', results);
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
          this.requests = results['requests'];
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

  deleteRequest(id: number): void {
    this._userService.deleteRequest(id).subscribe(results => {
      console.log('deleteRequest', results);
      if (results['success'] == 0) {
        this.toastr.error(results['message'], 'Attention', {
          timeOut: 1500,
          progressBar: true
        }).onHidden.subscribe(() => {
          this.loading = false;
        });
      } else {
        this.getRequests();
      }
    }, error => {
      this.toastr.error(error.error['message'], 'Attention', {
        timeOut: 1500,
        progressBar: true
      }).onHidden.subscribe(() => {
        this.loading = false;
      });
    });
  }

  editRequest(id: number): void {
    this.requestId = id;
    this.isOpen = true;
    console.log('requestId', this.requestId);
    console.log('isOpen', this.isOpen);
    $('#editRequest').modal('show');
  }

  closeModal(event?: any): void {
    console.log('closeModal', event);
    if (event === 0) {
      $('#createRequest').modal('hide');
    } else if (event === 1) {
      $('#editRequest').modal('hide');
      this.isOpen = false;
    }
    this.getRequests();
  }

}
