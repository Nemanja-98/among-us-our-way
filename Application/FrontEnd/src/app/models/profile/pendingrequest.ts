import { User } from "../user";

export class PendingRequest {
    id: number;
    userSentRef: string;
    userSent: User;
    userReceivedRef: string;
    userReceived: User;
  


    constructor(){
        this.userSentRef = null;
        this.userSent = null;
        this.userReceivedRef = null;
        this.userReceived = null;
    }
}