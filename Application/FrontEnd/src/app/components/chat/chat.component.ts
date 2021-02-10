import { Component, OnInit } from '@angular/core';
import * as signalR from "@aspnet/signalr";  
import { Friend } from 'src/app/models/profile/friend';
import { User } from 'src/app/models/user';
import { Message } from 'src/app/models/message/message';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit {
  public connection =  new signalR.HubConnectionBuilder()
  .withUrl("/messages")
  .build();
  public messages: Array<Message> = [];
  

  constructor() { }

  ngOnInit(): void {
    this.connection.on("ReceiveMessage", (msg)=>{
      this.messages.push(msg);
    })

    this.connection.start()
    .catch( (err)=>{
      return console.error(err.toString());
    });

  }

  sendMessage(event){
    console.log("ev",event,event.target.parentNode.previousSibling.value);//,event.target.parentNode.previousElement.value);
    let message;
    if(event)
      message  = event.target.parentNode.previousSibling.value;//document.getElementById('btn-input').textContent;
    console.log("click",message);
    this.connection.invoke("SendMessageToAll", message).catch((err)=>{
      return console.error(err.toString());
    });
  }

  setTextContent(text){
  }

}
