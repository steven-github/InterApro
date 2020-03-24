import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';
import { UserService } from './user.service';
import { ToastrService } from 'ngx-toastr';


@Injectable()
export class AuthGuardService implements CanActivate {

  constructor(private _userService: UserService, private toastr: ToastrService, private _router: Router) {

  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree {
    if (!this._userService.isUserLoggedIn()) {
      this.toastr.error('You are not allowed to access this page. Go and login first.', 'Error', {
        timeOut: 5000,
        progressBar: true
      }).onHidden.subscribe(() => {});
      //this._router.navigate(["log-in"], { queryParams: { retUrl: route.url } });
      this._router.navigate(["log-in"]);
      return false;
    }

    return true;
  }

}
