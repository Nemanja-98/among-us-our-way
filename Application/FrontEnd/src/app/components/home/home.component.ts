import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { AccountService } from './../../services/account.service';
import { UserService } from './../../services/user.service';
import { State } from 'src/app/store/reducers/root.reducer';
import { User } from './../../models/user';
import { Store } from '@ngrx/store';
import { FriendsComponent } from '../friends/friends.component';
import { FriendListComponent } from '..//friends/friend-list/friend-list.component';
import { HttpClient } from '@angular/common/http';
import {ChatComponent} from 'src/app/components/chat/chat.component'
import * as signalR from '@aspnet/signalr';
import   { Friend } from '../../models/profile/friend'
// class Friend {
//   name: string;
//   status: string;
//   constructor(n: string, s: string) {
//     this.name = n;
//     this.status = s;
//   }
// }

// let mockFriends = [
//   new Friend('Jovan', 'in game'),
//   new Friend('5up', 'Offline'),
//   new Friend('Luka', 'Online'),
//   new Friend('Spikii', 'in game'),
// ];
@Component({
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent {
  //products$: Observable<Product[]>;
  loading = false;
  currentUser: User;
  userFromApi: User;
  friends: Friend[] ;
  textic = '';
  usersArray: Array<any> = []; //users
  token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6Imx1a2EiLCJleHAiOjE2MTMwNjY3NDgsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjUwMDEvIiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdDo4MDgwLyJ9.givlvRg_VMH51u9GpWjD5k2s4KEZkXs5FDHmn8v2cMg";

  public connection =  new signalR.HubConnectionBuilder()
  .withUrl("/friend", { accessTokenFactory: () => this.token })
  .build();
  public friend: Array<Friend> = [];

  constructor(
    private userService: UserService,
    private accountService: AccountService,
    private store: Store<State>,
    private http: HttpClient
  ) {
    this.currentUser = this.accountService.userValue;
    //  URL ZA BACKEND IZVUCI U NEKU PROM U ENV
    // this.http.get('SADASD').subscribe((data: any) => {
    //   // Populating usersArray with names from API
    //   data.json().forEach((element) => {
    //     this.usersArray.push(element.name);
    //   });
    // });

    this.connection.on("GetLiveFriends", (frd)=>{
      this.friend = frd;
    })

    this.connection.start()
    .catch( (err)=>{
      return console.error(err.toString());
    });
  }

  ngOnInit() {
    this.loading = true;
    setTimeout(() => {
      this.loading = false;
    }, 2000);
    // this.userService.getById(this.currentUser.id).pipe(first()).subscribe(user => {
    //     this.loading = false;
    //     this.userFromApi = user;
    // });
  }
  fun() {
    console.log('fun');
    this.textic = '1 ';
    //  document.getElementById('notificaiton').innerHTML += `
    //     <a type="button" class="btn btn-primary mt-2">
    //       Another 33% done
    //     </a>`;

    setTimeout(() => {
      this.textic = ' ';
    }, 1500);
  }
}
