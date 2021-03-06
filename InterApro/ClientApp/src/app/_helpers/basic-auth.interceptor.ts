import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';

import { UserService } from '../_services/user.service';

@Injectable()
export class BasicAuthInterceptor implements HttpInterceptor {
    constructor(private _userService: UserService) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        // add authorization header with basic auth credentials if available
        const currentUser = this._userService.currentUserValue;
        if (currentUser && currentUser.authData) {
            request = request.clone({
                setHeaders: { 
                    Authorization: `Basic ${currentUser.authData}`
                }
            });
        }

        return next.handle(request);
    }
}