import { Component } from '@angular/core';
import { LeagueService, AlertService } from '../../services/index';

@Component({
    selector: 'createleague',
    templateUrl: './createleague.component.html'
})
export class CreateLeagueComponent {
    model: any = {};
    created: boolean = false;

    constructor(private leagueService: LeagueService, private alertService: AlertService) { }

    createleague() {
        this.leagueService.create(this.model)
            .subscribe(
            data => {
                this.model.code = data.json().code;
                this.created = true;
            },
            error => {
                this.alertService.error(error._body);
            }

            );
    }
}