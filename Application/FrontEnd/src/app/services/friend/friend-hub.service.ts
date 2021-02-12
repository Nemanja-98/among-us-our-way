import { Injectable } from '@angular/core';
import * as signalR from '@aspnet/signalr';
import { Observable, of } from 'rxjs';
import { Friend } from 'src/app/models/profile/friend';

@Injectable({
  providedIn: 'root',
})
export class FriendHubService {
  public connection;
  public friend = [];

  constructor() {}

  beginConnection(token): Observable<Array<string>> {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl('/friend', { accessTokenFactory: () => token,
      transport: signalR.HttpTransportType.LongPolling
    }).configureLogging(signalR.LogLevel.Information)
      .build();

    const  start = async () => {
      try {
        await this.connection.start();
        console.log('SignalR Connected. for friendhub');
      } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
      }
    }

    this.connection.onclose(start);

    // Start the connection.
    start();

    this.connection.on('GetLiveFriends', (frd) => {
      console.log('frd is', frd);
      frd.value.friendsList.forEach((element) => {
        this.friend.push(element);
      });
      console.log('frdhub array', this.friend);
    });

    this.connection.on('UserConnected', (user) => {
      this.friend.push(user);
      console.log('friend hub user pushed', user,this.friend);
      
    });

    this.connection.on('UserDisconnected', (user) => {
      this.friend = this.friend.filter((el) => el != user);
      console.log('friend hub user removed', user, 'from this array', this.friend);
    });

    return of(this.friend);
  }
}
