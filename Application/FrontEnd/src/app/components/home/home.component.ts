import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { AccountService } from '../../services/account/account.service';
import { State } from 'src/app/store/reducers/root.reducer';
import { User } from './../../models/user';
import { Store } from '@ngrx/store';
import { FriendsComponent } from '../friends/friends.component';
import { FriendListComponent } from '..//friends/friend-list/friend-list.component';
import {ChatComponent} from 'src/app/components/chat/chat.component'
import * as signalR from '@aspnet/signalr';
import   { Friend } from '../../models/profile/friend'
import { FriendHubService } from 'src/app/services/friend/friend-hub.service';
import { ChatHubService } from 'src/app/services/chat/chat-hub.service';
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
  token = localStorage.getItem('token');
  public friend: Array<string>= [];
  public chat: Array<string>= [];

  constructor(
    private accountService: AccountService,
    private store: Store<State>,
    private friendHub: FriendHubService,
    private chatHub: ChatHubService,
  ) {
    this.currentUser = this.accountService.userValue;
    this.token = localStorage.getItem('token');
    console.log("home token",this.token);
    this.chatHub.beginConnection(this.token).subscribe(data => {
      this.chat = data;
      console.log("home compoenent chat array",this.chat);
      
      this.friendHub.beginConnection(this.token).subscribe(data => {
      this.friend = data;
      console.log("home compoenent friend array",this.friend)
    });
    })
  }

  ngOnInit() {
    this.loading = true;
    setTimeout(() => {
      this.loading = false;
    }, 1500);
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
