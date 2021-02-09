import { User } from "../user";
import { GameHistory } from "./gamehistory";
import { PlayerActions } from "./playeractions";

export class Game {
    id: number;
    dateStarted: string; // gggg-mm-ddThh:mm:ss
    actions: PlayerActions;
    players: GameHistory;
    
    constructor(){
        this.dateStarted = "";
        this.actions = null;
        this.players= null;
    }
}