import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { LeagueService, AlertService } from '../../services/index';
import { League } from '../../models/index';

@Component({
    selector: 'leagues',
    templateUrl: './leagues.component.html'
})
export class LeaguesComponent {

    leagues: League[] = [];
    constructor(private leagueService: LeagueService,
        private alertService: AlertService,
        private router: Router
    ) { }

    ngOnInit() {
        this.loadAllLeagues();
    }

    private loadAllLeagues() {
        this.leagueService.getJoined().subscribe(leagues => { this.leagues = leagues; });
    }

    manage(leagueid: string) {
        this.router.navigate(['league/manage', leagueid]);
    }

    leave(leagueid: string) {
        this.leagueService.leave(leagueid)
            .subscribe(
                    data => { },
                    error => {
                        this.alertService.error(error._body);
        });
    }
}