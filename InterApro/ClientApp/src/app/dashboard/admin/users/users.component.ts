import { Component, OnInit } from '@angular/core';
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
  rol = [
    { id: -1, name: 'Admin' },
    { id: 0, name: 'User' },
    { id: 1, name: 'Boss' },
    { id: 2, name: 'Financial Approver 1' },
    { id: 3, name: 'Financial Approver 2' },
    { id: 4, name: 'Financial Approver 3' }
  ];

  constructor(private _userService: UserService) {
    this.getUsers();
  }

  ngOnInit(): void {
    $('[data-toggle="tooltip"]').tooltip();
  }

  getUsers() {
    //this.users = this._userService.getUsers();
    this._userService.getUsers().subscribe(res => {
      this.users = res;
    });
  }

  editUser(id) {
    console.log('id', id);
  }

  getRolName(id): string {
    let rol = this.rol.find(obj => obj.id == id);
    return rol.name;
  }

}
