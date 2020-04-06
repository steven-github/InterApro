import { Component } from '@angular/core';
import { UserService } from '../_services/user.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss']
})
export class NavMenuComponent {
  isExpanded = false;

  constructor(private _userService: UserService, private _router: Router) {
    if(this._userService.currentUserValue == null) {
      
    }
    console.log('currentUserValue', this._userService.currentUserValue);
    console.log('currentUser', this._userService.currentUser);
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  logout() {
    this._userService.logoutUser();
    this._router.navigate(['/log-in']);
  }
}
