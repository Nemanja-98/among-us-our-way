import { Role } from "./role";
import { Friend } from "./profile/friend";
import { GameHistory } from "./game/gamehistory";
import { PendingRequest } from "./profile/pendingrequest";

export class User {
    id: number;
    username: string;
    password: string;
    gamesPlayed: number;
    crewmateGames: number;
    impostorGames: number;
    crewmateWonGames: number;
    impostorWonGames: number;
    tasksCompleted: number;
    allTasksCompleted: number;
    kills: number;
    games: Array<GameHistory>;
    friends: Array<Friend>;
    sentRequests: Array<PendingRequest>;
    pendingReqeusts: Array<PendingRequest>;


    constructor(){
        this.username = "";
        this.password = "";
        this.gamesPlayed = 0;
        this.crewmateGames = 0;
        this.impostorGames = 0;
        this.crewmateWonGames = 0;
        this.impostorWonGames = 0;
        this.tasksCompleted = 0;
        this.allTasksCompleted = 0;
        this.kills = 0;
        this.games = new Array<GameHistory>();
        this.friends = new Array<Friend>();
        this.sentRequests = new Array<PendingRequest>();
        this.pendingReqeusts = new Array<PendingRequest>();
    }
}