import { Component, OnInit, ElementRef, ViewChild, EventEmitter } from '@angular/core';
import { UserService } from '../../../_services/user.service';
import { User } from '../../../interfaces';
import { ToastrService } from 'ngx-toastr';
declare var $: any;

@Component({
  selector: 'app-admin-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class AdminUsersComponent implements OnInit {

  users: User[];
  status = [
    { id: -1, name: 'Active' },
    { id: 0, name: 'Inactive' }
  ];
  rol = [
    { id: -1, name: 'Admin' },
    { id: 0, name: 'Buyer' },
    { id: 1, name: 'Boss' },
    { id: 2, name: 'Financial Approver 1' },
    { id: 3, name: 'Financial Approver 2' },
    { id: 4, name: 'Financial Approver 3' }
  ];
  loading: boolean = false;
  // test: boolean = true;
  // @ViewChild("createUserModal") createUserModal: ElementRef;

  constructor(private _userService: UserService, private toastr: ToastrService) {
    this.getUsers();
  }

  ngOnInit(): void {
    $('[data-toggle="tooltip"]').tooltip();
  }

  getUsers(): void {
    this.loading = true;
    this._userService.getUsers().subscribe(results => {
      if (results['success'] == 0) {
        this.toastr.error(results['message'], 'Error', {
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
          this.users = results['users'];
          this.loading = false;
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

  closeModal(event): void {
    $('#createUser').modal('hide');
    console.log('event', event);
    this.getUsers();
  }

  editUser(id: number): void {
    console.log('id', id);
  }

  deleteUser(id: number): void {
    this._userService.delete(id).subscribe(results => {
      if (results['success'] == 0) {
        this.toastr.error(results['message'], 'Error', {
          timeOut: 1500,
          progressBar: true
        }).onHidden.subscribe(() => {
          this.loading = false;
        });
      } else {
        this.getUsers();
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

  getRolName(id: number): string {
    let rol = this.rol.find(obj => obj.id == id);
    return rol.name;
  }

}
