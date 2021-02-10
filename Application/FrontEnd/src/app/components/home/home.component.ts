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

class Friend {
  name: string;
  status: string;
  constructor(n: string, s: string) {
    this.name = n;
    this.status = s;
  }
}

let mockFriends = [
  new Friend('Jovan', 'in game'),
  new Friend('5up', 'Offline'),
  new Friend('Luka', 'Online'),
  new Friend('Spikii', 'in game'),
];
@Component({
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent {
  //products$: Observable<Product[]>;
  loading = false;
  currentUser: User;
  userFromApi: User;
  friends: Friend[] = mockFriends;
  textic = '';
  usersArray: Array<any> = []; //users

  constructor(
    private userService: UserService,
    private accountService: AccountService,
    private store: Store<State>,
    private http: HttpClient
  ) {
    this.currentUser = this.accountService.userValue;
    //  URL ZA BACKEND IZVUCI U NEKU PROM U ENV
    this.http.get('SADASD').subscribe((data: any) => {
      // Populating usersArray with names from API
      data.json().forEach((element) => {
        this.usersArray.push(element.name);
      });
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
