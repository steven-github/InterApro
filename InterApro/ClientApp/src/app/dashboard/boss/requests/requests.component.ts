import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/_services/user.service';
import { FormBuilder } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-boss-requests',
  templateUrl: './requests.component.html',
  styleUrls: ['./requests.component.scss']
})
export class RequestsBossComponent implements OnInit {

  requests: Request[] = new Array();
  submitted = false;
  loading = false;

  constructor(private _userService: UserService, private formBuilder: FormBuilder, private toastr: ToastrService) {
    console.log('_userService.currentUserValue', _userService.currentUserValue);
    this.getRequestsById();
  }

  ngOnInit(): void {
    $('[data-toggle="tooltip"]').tooltip();
  }

  getRequestsById(): void {
    this.loading = true;
    this._userService.getRequestsById(this._userService.currentUserValue.userId).subscribe(results => {
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

}
