import { Component } from '@angular/core';
import { LeagueService, AlertService } from '../../services/index';

@Component({
    selector: 'joinleague',
    templateUrl: './joinleague.component.html'
})
export class JoinLeagueComponent {
    model: any = {};

    constructor(private leagueService: LeagueService, private alertService: AlertService) { }

    joinleague() {
        this.leagueService.find(this.model)
            .subscribe(
            data => {
                this.model.id = data.json().id;
                this.leagueService.join(this.model)
                    .subscribe(
                    data => {

                    },
                    error => {
                        this.alertService.error(error._body);
                    }

                    );
            },
            error => { this.alertService.error(error._body); }

            );
        
    }
}