
import { Injectable, Inject } from '@angular/core';
import { User, Requests } from '../interfaces';
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
  currentUserSubject: BehaviorSubject<User> = null;
  currentUser: Observable<User> = null;

  constructor(protected http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
    this.currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('currentUser')));
    this.currentUser = this.currentUserSubject.asObservable();
  }

  // ngOnInit() {
  //   console.log('ngOnInit');
  // }

  // ngAfterViewInit() {
  //   console.log('ngAfterViewInit');
  // }

  // ngAfterViewChecked() {
  //   console.log('ngAfterViewChecked');
  // }

  // ngOnChanges() {
  //   console.log('ngOnChanges');
  // }

  // ngDoCheck() {
  //   console.log('ngDoCheck');
  // }

  // viewToModelUpdate(newValue: any): void {
  //   console.log('viewToModelUpdate',  newValue);
  // }

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.baseUrl + 'api/users');
  }

  getBosses(): Observable<User[]> {
    return this.http.get<User[]>(this.baseUrl + 'api/users/bosses');
  }

  getRequests(): Observable<User[]> {
    return this.http.get<User[]>(this.baseUrl + 'api/users/requests');
  }

  getUser(id): Observable<User[]> {
    return this.http.get<User[]>(this.baseUrl + 'api/users/' + id);
  }

  getRequest(id): Observable<Requests[]> {
    return this.http.get<Requests[]>(this.baseUrl + 'api/users/request/' + id);
  }

  getRequestsAssignedToUser(id): Observable<Requests[]> {
    return this.http.get<Requests[]>(this.baseUrl + 'api/users/requests-assigned-to-user/' + id);
  }

  getAssignee(id): Observable<User[]> {
    return this.http.get<User[]>(this.baseUrl + 'api/users/assignee/' + id);
  }

  getBoss(id): Observable<User[]> {
    return this.http.get<User[]>(this.baseUrl + 'api/users/boss/' + id);
  }


  public get currentUserValue(): User {
    return this.currentUserSubject.value;
  }

  isUserLoggedIn(): boolean {
    // const currentUser = localStorage.getItem('currentUser');
    const currentUser = this.currentUserValue;
    if (currentUser && currentUser.authData) {
      return true;
    }
    return false;
  }

  createUser(form: any): Observable<any> {
    return this.http.post<Response>(this.baseUrl + 'api/users/create-user', {
      'FirstName': form.controls.firstName.value,
      'LastName': form.controls.lastName.value,
      'Email': form.controls.email.value,
      'Username': form.controls.username.value,
      'Password': form.controls.password.value,
      'Status': Number(form.controls.status.value),
      'Rol': Number(form.controls.rol.value),
      'BossId': Number(form.controls.bossId.value),
    }, httpOptions);
  }

  editUser(id: number, form: any): Observable<any> {
    return this.http.put(this.baseUrl + 'api/users/' + id, {
      'Id': id,
      'FirstName': form.controls.firstName.value,
      'LastName': form.controls.lastName.value,
      'Email': form.controls.email.value,
      'Username': form.controls.username.value,
      'Password': form.controls.password.value,
      'Status': Number(form.controls.status.value),
      'Rol': Number(form.controls.rol.value)
    }, httpOptions);
  }

  editRequestByCreator(id: number, form: any): Observable<any> {
    return this.http.put(this.baseUrl + 'api/users/edit-request-by-creator/' + id, {
      'Id': id,
      'Price': form.controls.price.value,
      'Description': form.controls.description.value
    }, httpOptions);
  }

  editRequestByInternal(requestId: number, approve: boolean, assigneeId: number): Observable<any> {
    return this.http.put(this.baseUrl + 'api/users/edit-request-by-internal/' + requestId, {
      'Approve': approve,
      'AssigneeId': assigneeId
    }, httpOptions);
  }

  deleteUser(id: number): Observable<any> {
    return this.http.delete(this.baseUrl + 'api/users/' + id, httpOptions);
  }

  deleteRequestById(id: number): Observable<any> {
    return this.http.delete(this.baseUrl + 'api/users/delete-request-by-id/' + id, httpOptions);
  }

  loginUser(form: any): Observable<Response> {
    return this.http.post<any>(this.baseUrl + 'api/users/login', {
      'Username': form.controls.username.value,
      'Password': form.controls.password.value
    }, httpOptions)
      .pipe(map(user => {
        // store user details and basic auth credentials in local storage to keep user logged in between page refreshes
        user.authData = window.btoa(form.controls.username.value + ':' + form.controls.password.value);
        localStorage.setItem('currentUser', JSON.stringify(user));
        this.currentUser = localStorage.getItem('currentUser') ? JSON.parse(localStorage.getItem('currentUser')) : null;
        this.currentUserSubject.next(user);
        return user;
      }));
  }

  logoutUser() {
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
    this.currentUser = this.currentUserSubject;
  }

  createRequest(form: any) {
    console.log('createRequest', form);
    return this.http.post<Response>(this.baseUrl + 'api/users/create-request', {
      'userId': form.controls.userId.value,
      'FirstName': form.controls.firstName.value,
      'LastName': form.controls.lastName.value,
      'Email': form.controls.email.value,
      'Username': form.controls.username.value,
      'assigneeId': form.controls.assigneeId.value,
      'assigneeName': form.controls.assigneeName.value,
      'price': form.controls.price.value,
      'description': form.controls.description.value
      // 'boss': form.controls.boss.value,      
      // 'Rol': Number(form.controls.rol.value)
    }, httpOptions);
  }
}
