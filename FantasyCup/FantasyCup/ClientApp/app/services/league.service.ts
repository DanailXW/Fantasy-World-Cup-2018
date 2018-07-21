import { Injectable } from '@angular/core';

import { League, LeagueMember } from '../models/index';
import { ApiService } from './api.service';

@Injectable()
export class LeagueService {

    constructor(
        private apiService: ApiService
    ) { }

    getJoined() {
        return this.apiService.http_get('/api/leagues/entered', true);
    }

    get(id: string) {
        return this.apiService.http_get('/api/leagues/' + id, true);
    }

    getDefault() {
        return this.apiService.http_get('/api/leagues/default', true);
    }

    create(league: League) {
        return this.apiService.http_post('/api/leagues/create', league, true);
    }

    update(league: League) {
        return this.apiService.http_post('/api/leagues/update', league, true);
    }

    delete(league: League) {
        return this.apiService.http_post('/api/leagues/delete', league, true);
    }

    join(league: League) {
        return this.apiService.http_post('/api/leagues/join', league, true);
    }

    leave(id: string) {
        return this.apiService.http_post('/api/leagues/leave/' + id, null, true);
    }

    find(league: League) {
        return this.apiService.http_post('/api/leagues/find', league, true);
    }

    getMembers(id: string) {
        return this.apiService.http_get('/api/leagues/' + id + '/members', true);
    }

    updateMembers(leagueId: string, members: LeagueMember[]) {
        return this.apiService.http_post('/api/leagues/' + leagueId + '/members/update', members, true);
    }

    getLeaderboard(leagueId: string) {
        return this.apiService.http_get('/api/leagues/' + leagueId + '/standings', true);
    }
}