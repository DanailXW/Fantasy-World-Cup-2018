import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions, Response } from '@angular/http';

import { AppConfig } from '../app.config';

@Injectable()
export class BetService {

    constructor(
        private http: Http,
        private config: AppConfig
    ) { }

    getGames() {
        return this.http.get(this.config.apiUrl + '/api/bets/games', this.jwt()).map((response: Response) => response.json());
    }

    placeGameBets(gameBets: any) {
        return this.http.post(this.config.apiUrl + '/api/bets/games/place', gameBets, this.jwt());
    }

    private jwt() {
        // create authorization header with jwt token
        let userFromStorage = localStorage.getItem('FantasyUser');
        let currentUser;

        if (userFromStorage)
            currentUser = JSON.parse(userFromStorage);

        if (currentUser && currentUser.token) {
            let headers = new Headers({ 'Authorization': 'Bearer ' + currentUser.token });
            return new RequestOptions({ headers: headers });
        }
    }
}