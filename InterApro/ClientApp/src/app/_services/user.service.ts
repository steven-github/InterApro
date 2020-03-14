
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

  constructor(protected http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.baseUrl + 'api/users');
  }

  saveUser(form) {
    console.log('form', form.controls);
    this.http.post<Response>(this.baseUrl + 'api/users', {
      'FirstName': form.controls.firstName.value,
      'LastName': form.controls.lastName.value,
      'Email': form.controls.email.value,
      'Username': form.controls.username.value,
      'Password': form.controls.password.value,
      'Rol': form.controls.rol.value
    }, httpOptions).subscribe(result => {
      console.log('result', result);
    }, error => console.error('error', error));
  }
}
