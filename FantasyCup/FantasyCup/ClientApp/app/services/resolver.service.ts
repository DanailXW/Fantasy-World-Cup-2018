import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { CompetitionService } from './competition.service';
import { BetService } from './bet.service';

@Injectable()
export class TeamResolver implements Resolve<any> {

    constructor(private competitionService: CompetitionService) { }

    resolve(route: ActivatedRouteSnapshot) {
        return this.competitionService.getTeams();
    }
}

@Injectable()
export class PlayerResolver implements Resolve<any> {
    constructor(private competitionService: CompetitionService) { }

    resolve(route: ActivatedRouteSnapshot) {
        return this.competitionService.getPlayers();
    }
}

@Injectable()
export class CompetitionBetsResolver implements Resolve<any> {
    constructor(private betService: BetService) { }

    resolve(route: ActivatedRouteSnapshot) {
        return this.betService.getCompetitionBets().catch(error => { return Observable.empty(); });
    }
}

@Injectable()
export class GameResolver implements Resolve<any> {
    constructor(private competitionService: CompetitionService) { }

    resolve(route: ActivatedRouteSnapshot) {
        return this.competitionService.getGames();
    }
}