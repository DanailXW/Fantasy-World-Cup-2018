import { Component, OnInit, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { BetService } from '../../services/bet.service';
import { AlertService } from '../../services/alert.service';
import { GameBet } from '../../models/index';

@Component({
    selector: 'competitionbets',
    templateUrl: './competitionbets.component.html'
})
export class CompetitionBetsComponent {

    @ViewChild('cbForm') form: any;
    teams: any[] = [];
    players: any[] = [];
    playersFiltered: any[] = [];
    userSelections: any[] = [];
    selectedTeam: any = {};
    selectedPlayerTeam: any = {};
    selectedPlayer: any = {};

    constructor(private betService: BetService,
        private alertService: AlertService,
        private router: Router,
        private route: ActivatedRoute
    ) {
    }


    ngOnInit() {
        this.teams = this.route.snapshot.data['teams'];
        this.players = this.route.snapshot.data['players'];
        this.loadSelections();
    }

    private loadSelections() {
        let data = this.route.snapshot.data['bets'];
        

        let championBet = data.find((x: any) => x.betType.name == 'COMPETITION_CHAMPION');
        if (championBet)
        {
            this.selectedTeam = this.teams.find(x => x.id == championBet.selectionId);
        }
        else
            this.selectedTeam.name = '-- select --';

        let scorerBet = data.find((x: any) => x.betType.name == 'COMPETITION_TOP_SCORER');
        if (scorerBet) {
            this.selectedPlayer = this.players.find(x => x.id == scorerBet.selectionId);
            this.selectedPlayerTeam = this.teams.find(x => x.id == this.selectedPlayer.team.id);
            this.playersFiltered = this.players.filter(x => x.teamId == this.selectedPlayer.team.id);
        }
        else {
            this.selectedPlayerTeam.name = '-- select --';
            this.selectedPlayer.name = '-- select --';
        }

    }

    teamSelected(team: any) {
        this.selectedTeam = team;
        this.form.form.markAsDirty();
    }

    playerTeamSelected(team: any) {
        this.selectedPlayerTeam = team;
        this.playersFiltered = this.players.filter(x => x.team.id == team.id);
        this.selectedPlayer = {};
        this.selectedPlayer.name = '-- select --';
        this.form.form.markAsDirty();
    }

    playerSelected(player: any) {
        this.selectedPlayer = player;
        this.form.form.markAsDirty();
    }

    saveBets() {
        let bets: any[] = [];

        if (this.selectedTeam.id) {
            let championBet: any = {};
            championBet.betType = 'COMPETITION_CHAMPION';
            championBet.selectionId = this.selectedTeam.id;

            bets.push(championBet);
        }

        if (this.selectedPlayer.id) {
            let scorerBet: any = {};
            scorerBet.betType = 'COMPETITION_TOP_SCORER';
            scorerBet.selectionId = this.selectedPlayer.id;

            bets.push(scorerBet);
        }

        this.betService.placeCompetitionBets(bets).subscribe(
            data => { this.alertService.success('Saved successfully!'); this.form.form.markAsPristine(); },
            error => { this.alertService.error(error._body); }
        );
    }
}