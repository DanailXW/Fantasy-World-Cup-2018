import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { LeagueService, AlertService } from '../../services/index';

@Component({
    selector: 'joinleague',
    templateUrl: './joinleague.component.html',
    styleUrls: ['../../styles/forms.css']
})
export class JoinLeagueComponent {
    model: any = {};

    constructor(private leagueService: LeagueService, private alertService: AlertService, private router: Router) { }

    joinleague() {
        this.leagueService.find(this.model)
            .subscribe(
            data => {
                this.model.id = data.json().id;
                this.leagueService.join(this.model)
                    .subscribe(
                    data => {
                        this.alertService.success('Joined successfully!');
                        this.router.navigate(['leagues']);
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