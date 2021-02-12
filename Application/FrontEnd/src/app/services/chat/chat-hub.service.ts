import { Injectable } from '@angular/core';
import * as signalR from '@aspnet/signalr';
import { of } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ChatHubService {
  constructor() {}

  public connection;
  public users = [];

  beginConnection(token) {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl('/chat', { accessTokenFactory: () => token,
        transport: signalR.HttpTransportType.LongPolling })
      .configureLogging(signalR.LogLevel.Information)
      .build();

    const start = async () => {
      try {
        await this.connection.start();
        console.log('SignalR Connected for chathub.');
      } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
      }
    }

    this.connection.onclose(start);

    // Start the connection.
    start();

    this.connection.on('GetLiveUsers', (users) => {
      console.log('chatHub users is', users);

      users.value.userList.forEach((element) => {
        this.users.push(element);
      });
      console.log('chathub array', this.users);
    });

    this.connection.on('UserConnected', (user) => {
      this.users.push(user);
      console.log('chat hub user pushed', user);
    });
    
    this.connection.on('UserDisconnected', (user) => {
      this.users = this.users.filter((el) => el != user);
      console.log('chathub user removed', user, 'from this array', this.users);
    });

    this.connection.on('ReceiveMessage', (msg) => {
      console.log("msg received by chathub",msg);
    });

    return of(this.users);
  }
}
