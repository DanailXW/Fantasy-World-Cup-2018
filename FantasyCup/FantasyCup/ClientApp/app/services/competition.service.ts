import { Injectable } from '@angular/core';
import { ApiService } from './api.service';

@Injectable()
export class CompetitionService {

    constructor(
        private api: ApiService
    ) { }

    getTeams() {
        return this.api.http_get('/api/1/teams', false);
    }

    getPlayers() {
        return this.api.http_get('/api/1/players', false);
    }

    getPlayersByTeam(teamId: number) {
        return this.api.http_get('/api/1/team/' + teamId.toString() + '/players', false);
    }

    getGames() {
        return this.api.http_get('/api/1/games', false);
    }

    getPlayerStats() {
        return this.api.http_get('/api/1/playerstats', false);
    }

    getGroupStandings() {
        return this.api.http_get('/api/1/groupstandings', false);
    }
}