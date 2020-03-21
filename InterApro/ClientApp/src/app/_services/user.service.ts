
import { Injectable, Inject } from '@angular/core';
import { User } from '../interfaces';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json',
    'Authorization': 'my-auth-token'
  })
}

@Injectable({
  providedIn: 'root',
})

export class UserService {

  baseUrl: string;
  currentUser: any;

  constructor(protected http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
    this.currentUser = localStorage.getItem('currentUser') ? JSON.parse(localStorage.getItem('currentUser')) : '';
    console.log('UserService', this.currentUser);
  }

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.baseUrl + 'api/users');
  }

  isUserLoggedIn(): boolean {
    const currentUser = localStorage.getItem('currentUser');
    console.log('currentUser', currentUser);
    if (currentUser) {
      return true;
    }
    return false;
  }

  isAdmin(): boolean {
    const currentUser = localStorage.getItem('currentUser');
    console.log('currentUser', currentUser);
    if (currentUser['isLogged'] && currentUser['rol'] == -1) {
      return true;
    }
    return false;
  }



  //register(form) {
  //  this.http.post<Response>(this.baseUrl + 'api/users', {
  //    'FirstName': form.controls.firstName.value,
  //    'LastName': form.controls.lastName.value,
  //    'Email': form.controls.email.value,
  //    'Username': form.controls.username.value,
  //    'Password': form.controls.password.value,
  //    'Rol': form.controls.rol.value
  //  }, httpOptions).subscribe(result => {
  //    console.log('result', result);
  //  }, error => console.error('error', error));
  //}

  create(form): Observable<Response> {
    return this.http.post<Response>(this.baseUrl + 'api/users/create', {
      'FirstName': form.controls.firstName.value,
      'LastName': form.controls.lastName.value,
      'Email': form.controls.email.value,
      'Username': form.controls.username.value,
      'Password': form.controls.password.value,
      'Rol': form.controls.rol.value
    }, httpOptions);
  }

  login(form): Observable<Response> {
    return this.http.post<Response>(this.baseUrl + 'api/users/login', {
      'Username': form.controls.username.value,
      'Password': form.controls.password.value
    }, httpOptions);
  }

  logout() {
    // remove user data from local storage for log out
    localStorage.removeItem('currentUser');
    this.currentUser = null;
  }
}
