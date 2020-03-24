import { Component, OnInit, ElementRef, ViewChild, EventEmitter } from '@angular/core';
import { UserService } from '../../../_services/user.service';
import { User } from '../../../interfaces';
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
  // test: boolean = true;
  // @ViewChild("createUserModal") createUserModal: ElementRef;

  constructor(private _userService: UserService) {
    this.getUsers();
  }

  ngOnInit(): void {
    $('[data-toggle="tooltip"]').tooltip();
  }

  getUsers(): void {
    //this.users = this._userService.getUsers();
    this._userService.getUsers().subscribe(res => {
      console.log('getUsers', res);
      this.users = res;
    });
  }

  closeModal(event): void {
    $('#createUser').modal('hide');
    console.log('event', event);
    this.getUsers();
  //   setTimeout (() => {
  //     console.log("Hello from setTimeout");
  //     this.getUsers();
  //  }, 1000);
  }

  editUser(id: number): void {
    console.log('id', id);
  }

  deleteUser(id: number): void {
    this._userService.delete(id).subscribe(res => {
      console.log('delete', res);
    });
  }

  getRolName(id: number): string {
    let rol = this.rol.find(obj => obj.id == id);
    return rol.name;
  }

}
