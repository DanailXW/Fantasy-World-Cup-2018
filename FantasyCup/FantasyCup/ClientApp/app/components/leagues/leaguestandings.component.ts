import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { LeagueService, AlertService } from '../../services/index';
import { League, LeagueMember } from '../../models/index';

@Component({
    selector: 'leaguestandings',
    templateUrl: './leaguestandings.component.html',
    styleUrls: ['../../styles/tables.css']
})
export class LeagueStandingsComponent {

    standings: any[] = [];
    userId: number;

    constructor(private leagueService: LeagueService,
        private alertService: AlertService,
        private route: ActivatedRoute
    ) { }

    ngOnInit() {
        let leagueId = this.route.snapshot.paramMap.get('id') || '';
        this.loadStandings(leagueId);
        this.getUserId();
    }

    loadStandings(leagueid: string) {
        this.leagueService
            .getLeaderboard(leagueid)
            .subscribe(
            data => {
                this.standings = data.sort((left: any, right: any): number =>
                {
                    if (left.points < right.points) return 1;
                    if (left.points > right.points) return -1;
                    return 0;
                });
            },
                    error => { this.alertService.error(error._body); }
            );
    }

    getUserId() {
        let userFromStorage = localStorage.getItem('FantasyUser');
        let currentUser;

        if (userFromStorage)
            currentUser = JSON.parse(userFromStorage);

        this.userId = currentUser && currentUser.id;
    }


}