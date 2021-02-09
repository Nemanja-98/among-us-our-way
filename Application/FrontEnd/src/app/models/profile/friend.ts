import { User } from "../user";
export class Friend {
    id: number;
    user1Ref: string;
    user1: User;
    user2Ref: string;

    constructor(){
        this.user1Ref = "";
        this.user1 = new User();
        this.user2Ref = "";
    }
}