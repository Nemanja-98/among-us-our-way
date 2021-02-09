import { User } from "../user";
import { Game } from "./game";

export class PlayerActions {
    id: number;
    userId: string;
    gameId: number;
    game: Game;
    action: number; // 0-start task  1-finish task 2-kill 3-vent in 4-vent out 5-body reported 6-emergency called
    time: string;
    constructor(){
        this.userId = "";
        this.gameId = null;
        this.game= null;
        this.action= null;
        this.time= "";
    }
}