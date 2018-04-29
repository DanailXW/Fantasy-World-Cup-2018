import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { BetService, AlertService } from '../../services/index';
import { GameBet } from '../../models/index';

@Component({
    selector: 'gamebets',
    templateUrl: './gamebets.component.html'
})
export class GameBetsComponent {

    games: any[] = [];

    constructor(private betService: BetService,
        private alertService: AlertService,
        private router: Router
    ) { }

    ngOnInit() {
        this.loadGames();
    }

    private loadGames() {
        this.betService.getGames().subscribe(data => { this.games = data; });        
    }

    saveBets() {
        this.betService
                .placeGameBets(this.games.filter(x => x.changed && x.scoreA != null && x.scoreB != null))
                .subscribe(
                    data => { },
                    error => { this.alertService.error(error._body); }
                );

        this.games.forEach(x => { x.changed = false; });
    }

    rowChanged(idx: number) {
        this.games[idx].changed = true;
    }
}