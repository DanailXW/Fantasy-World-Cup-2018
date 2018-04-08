import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions, Response } from '@angular/http';

import { AppConfig } from '../app.config';
import { League, LeagueMember } from '../models/index';

@Injectable()
export class LeagueService {

    constructor(
        private http: Http,
        private config: AppConfig
    ) { }

    getAll() {
        return this.http.get(this.config.apiUrl + '/api/leagues', this.jwt()).map((response: Response) => response.json());
    }

    getJoined() {
        return this.http.get(this.config.apiUrl + '/api/leagues/entered', this.jwt()).map((response: Response) => response.json());
    }

    get(id: string) {
        return this.http.get(this.config.apiUrl + '/api/leagues/' + id, this.jwt()).map((response: Response) => response.json());
    }

    create(league: League) {
        return this.http.post(this.config.apiUrl + '/api/leagues/create', league, this.jwt());
    }

    update(league: League) {
        return this.http.post(this.config.apiUrl + '/api/leagues/update', league, this.jwt());
    }

    join(league: League) {
        return this.http.post(this.config.apiUrl + '/api/leagues/join', league, this.jwt());
    }

    leave(id: string) {
        return this.http.post(this.config.apiUrl + '/api/leagues/leave/' + id, null, this.jwt());
    }

    find(league: League) {
        return this.http.post(this.config.apiUrl + '/api/leagues/find', league, this.jwt());
    }

    getMembers(id: string) {
        return this.http.get(this.config.apiUrl + '/api/leagues/' + id + '/members', this.jwt());
    }

    updateMembers(leagueId: string, members: LeagueMember[]) {
        return this.http.post(this.config.apiUrl + '/api/leagues/' + leagueId + '/members/update', members, this.jwt());    
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