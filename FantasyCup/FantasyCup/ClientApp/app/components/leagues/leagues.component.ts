import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { LeagueService, AlertService } from '../../services/index';

@Component({
    selector: 'leagues',
    templateUrl: './leagues.component.html'
})
export class LeaguesComponent {

    leagues: any[] = [];
    defaultLeague: any;

    constructor(private leagueService: LeagueService,
        private alertService: AlertService,
        private router: Router
    ) { }

    ngOnInit() {
        this.loadAllLeagues();
        //this.getDefaultLeague();
    }

    private loadAllLeagues() {
        this.leagueService.getJoined().subscribe(leagues => { this.leagues = leagues; this.getDefaultLeague(); });
    }

    private getDefaultLeague() {
        this.leagueService.getDefault().subscribe(league => this.defaultLeague = league);
    }

    manage(leagueid: string) {
        this.router.navigate(['league/manage', leagueid]);
    }

    leave(leagueid: string) {
        this.leagueService.leave(leagueid)
            .subscribe(
            data => {
                this.loadAllLeagues();
                this.alertService.success('You left the league successfully!');
            },
                    error => {
                        this.alertService.error(error._body);
        });
    }

    standings(leagueid: string) {
        this.router.navigate(['league/standings', leagueid]);
    }

    create() {
        this.router.navigate(['league/create']);
    }

    join() {
        this.router.navigate(['league/join']);
    }
}