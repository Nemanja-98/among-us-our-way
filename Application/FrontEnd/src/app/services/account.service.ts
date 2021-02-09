
import { environment } from './../../environments/environment';
import { State } from '../store/reducers/root.reducer';
import { BehaviorSubject, Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { UserService } from './user.service';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { User } from './../models/user';
import { map } from 'rxjs/operators';

import { Store } from '@ngrx/store';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  private userSubject: BehaviorSubject<User>;
  public user: Observable<User>;

  constructor(
    private router: Router,
    private http: HttpClient,
    private store: Store<State>,
    private userService: UserService
  ) {
    this.userSubject = new BehaviorSubject<User>(
      JSON.parse(localStorage.getItem('currentUser'))
    );
    this.user = this.userSubject.asObservable();
  }

  public get userValue(): User {
    return this.userSubject.value;
  }

  async login(username: string, password: string) {
    console.log("logging in");
    // console.log("sending http ");
    // let niz = await this.http.get('https://192.168.1.102:5001/user/getusers',{headers: {
    //   'Access-Control-Allow-Origin': '*',
    //   'Content-Type': 'application/json',
    //   'Accept': 'application/json'
    // }});
    //console.log("niz",niz);
    const post =  this.http
    .post('/user/login', {
       "Username":username,
      "Password": password,
    },
    {headers: {
      'Access-Control-Allow-Origin': '*',
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    }});;
    post.toPromise().then(data => {
      console.log("ejta",data);
    })
    return  post;
      // .pipe(map((token) => {
      //     if ( token ) { //&& user.token) {
      //       console.log("got my token",token);
      //       //localStorage.setItem('currentUser', JSON.stringify(user));
      //       localStorage.setItem('token', token);
      //       //this.http.post()
      //       //this.userSubject.next(token);
      //     }
      //     return token;
      //   })
      // );
  }

   logout() {
//     localStorage.removeItem('currentUser');
//     this.userSubject.next(null);
//     this.router.navigate(['/account/login']);
   }

//   register(user: User) {
    
//     return this.http.post(`${environment.apiUrl}/user/addUser`, user);
//   }

//   registerToStore(user: User) {
//     // user.role = Role.User;
//     // user.boughtItemId = [];
//     this.store.dispatch(new addUser(user));
//     this.store.dispatch(new loadUsers());
//   }

   getAll() {
//     return this.http.get<User[]>(`${environment.apiUrl}/users`);
   }

//   getById(id: string) {
//     return this.http.get<User>(`${environment.apiUrl}/users/${id}`);
//   }

//   update(id, params) {
//     return this.http.put(`${environment.apiUrl}/users/${id}`, params).pipe(
//       map((x) => {
//         if (id == this.userValue.id) {
//           const user = { ...this.userValue, ...params };
//           localStorage.setItem('currentUser', JSON.stringify(user));

//           this.userSubject.next(user);
//         }
//         return x;
//       })
//     );
//   }

   delete(id: string) {
//     return this.http.delete(`${environment.apiUrl}/users/${id}`).pipe(
//       map((x) => {
//         if (id == this.userValue.id.toString()) {
//           this.logout();
//         }
//         return x;
//       })
//     );
   }
}
