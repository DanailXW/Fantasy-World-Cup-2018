import { Injectable } from '@angular/core';
import { ApiService } from './api.service';

@Injectable()
export class BetService {

    constructor(
        private api: ApiService
    ) { }

    getGames() {
        return this.api.http_get('/api/bets/games', true);
    }

    placeGameBets(gameBets: any) {
        return this.api.http_post('/api/bets/games/place', gameBets, true);
    }

    getCompetitionBets() {
        return this.api.http_get('/api/bets/1/competitionbets', true);
    }

    placeCompetitionBets(competitionBets: any) {
        return this.api.http_post('/api/bets/1/place', competitionBets, true);
    }

    getOthersBets(gameId: string) {
        return this.api.http_get('/api/bets/game/' + gameId + '/others', true);
    }
}