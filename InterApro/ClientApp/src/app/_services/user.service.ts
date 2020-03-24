
import { Injectable, Inject } from '@angular/core';
import { User } from '../interfaces';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';

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
  //currentUser: any;
  currentUserSubject: BehaviorSubject<User>;
  currentUser: Observable<User>;

  constructor(protected http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
    //this.currentUser = localStorage.getItem('currentUser') ? JSON.parse(localStorage.getItem('currentUser')) : '';
    //console.log('UserService', this.currentUser);
    this.currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('currentUser')));
    this.currentUser = this.currentUserSubject.asObservable();
  }

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.baseUrl + 'api/users');
  }

  public get currentUserValue(): User {
    return this.currentUserSubject.value;
}

  isUserLoggedIn(): boolean {
    // const currentUser = localStorage.getItem('currentUser');
    const currentUser = this.currentUserValue;
    console.log('isUserLoggedIn - currentUser', currentUser);
    if (currentUser && currentUser.authData) {
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

  create(form: any): Observable<any> {
    return this.http.post<Response>(this.baseUrl + 'api/users/create', {
      'FirstName': form.controls.firstName.value,
      'LastName': form.controls.lastName.value,
      'Email': form.controls.email.value,
      'Username': form.controls.username.value,
      'Password': form.controls.password.value,
      'Status': form.controls.status.value,
      'Rol': form.controls.rol.value
    }, httpOptions);
  }

  delete(id: number): Observable<any> {
    return this.http.delete(this.baseUrl + 'api/users/' + id, httpOptions);
  }

  //login(form): Observable<Response> {
  //  return this.http.post<Response>(this.baseUrl + 'api/users/login', {
  //    'Username': form.controls.username.value,
  //    'Password': form.controls.password.value
  //  }, httpOptions);
  //}

  login(form: any): Observable<Response> {
    return this.http.post<any>(this.baseUrl + 'api/users/login', {
      'Username': form.controls.username.value,
      'Password': form.controls.password.value
    }, httpOptions)
      .pipe(map(user => {
        // store user details and basic auth credentials in local storage to keep user logged in between page refreshes
        user.authData = window.btoa(form.controls.username.value + ':' + form.controls.password.value);
        localStorage.setItem('currentUser', JSON.stringify(user));
        this.currentUserSubject.next(user);
        return user;
      }));
  }

  //logout() {
  //  // remove user data from local storage for log out
  //  localStorage.removeItem('currentUser');
  //  this.currentUser = null;
  //}

  logout() {
    // remove user from local storage to log user out
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
  }
}
