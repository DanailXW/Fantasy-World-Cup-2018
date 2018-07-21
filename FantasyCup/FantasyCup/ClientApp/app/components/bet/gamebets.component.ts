import { Component, OnInit, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { BetService, AlertService } from '../../services/index';
import { GameBet } from '../../models/index';


@Component({
    selector: 'gamebets',
    templateUrl: './gamebets.component.html',
    styleUrls: ['../../styles/forms.css']
})
export class GameBetsComponent {

    @ViewChild('f') form: any;
    games: any[] = [];
    pageItemsCount: number = 8;
    p: number = 1;

    constructor(private betService: BetService,
        private alertService: AlertService,
        private router: Router

    ) {
    }

    ngOnInit() {
        this.loadGames();
    }

    private loadGames() {
        this.betService.getGames().subscribe(data => {
            this.games = data.sort((left: any, right: any): number => {
                if (left.game.startDate > right.game.startDate) return 1;
                if (left.game.startDate < right.game.startDate) return -1;
                return 0;
            });
            this.p = Math.floor(data.filter((x: any) => x.game.state == 'Finished').length / this.pageItemsCount) + 1;
        });        
    }

    saveBets() {
        this.betService
                .placeGameBets(this.games.filter(x => x.changed))
            .subscribe(
            data => {
                this.alertService.success('Saved successfully!');
                this.games.forEach(x => { x.changed = false; });
                this.form.form.markAsPristine();
            },
                    error => { this.alertService.error(error._body); }
                );
    }

    rowChanged(item: any) {
        item.changed = true;
        if (item.game.stageType == 'Elimination' && item.scoreA != item.scoreB)
            item.winningTeamId = null;
    }

    progressChecked(item: any, e: Event, side: string) {
        item.changed = true;
        let checkbox = <HTMLInputElement>e.target;
        if (checkbox.checked && side == 'A')
            item.winningTeamId = item.game.teamA.id;
        else if (checkbox.checked && side == 'B')
            item.winningTeamId = item.game.teamB.id;
        else
            item.winningTeamId = null;
    }

    viewOthersBets(gameId: number) {
        this.router.navigate(['game', gameId, 'bets']);
    }
}