import { Component, OnInit } from '@angular/core';
import { CompetitionService, AlertService } from '../../services/index';

@Component({
    selector: 'playerstats',
    templateUrl: 'playerstats.component.html',
    styleUrls: ['../../styles/tables.css']
})
export class PlayerStatsComponent {

    players: any[] = [];

    constructor(private competitionService: CompetitionService,
        private alertService: AlertService
    ) { }

    ngOnInit() {
        this.loadPlayers();
    }

    private loadPlayers() {
        this.competitionService.getPlayerStats().subscribe(
            data => {
            this.players = data.sort((left: any, right: any): number => {
                if (left.goals < right.goals) return 1;
                if (left.goals > right.goals) return -1;
                return 0;
            }); },
            error => { this.alertService.error(error._body); }
        )
    }
}
