import { User } from "../user";
import { Game } from "./game";

export class GameHistory {
    id: number;
    userId: string;
    user: User;
    gameId: number;
    game: Game;

    constructor(){
        this.userId = "";
        this.user = new User();
        this.gameId= 0;
        this.game = new Game();
       
    }
}