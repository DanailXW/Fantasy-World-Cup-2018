import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { LeagueService, AlertService } from '../../services/index';
import { League, LeagueMember } from '../../models/index';

@Component({
    selector: 'manageleague',
    templateUrl: './manageleague.component.html'
})
export class ManageLeagueComponent {
    members: LeagueMember[] = [];
    leagueId: string;
    league: any = {};

    constructor(private leagueService: LeagueService,
        private alertService: AlertService,
        private route: ActivatedRoute
    ) { }

    ngOnInit() {
        this.leagueId = this.route.snapshot.paramMap.get('id') || '';
        this.loadLeague();
        this.loadMembers();
    }

    private loadLeague() {
        this.leagueService.get(this.leagueId)
            .subscribe(
                data => { this.league = data;  },
                error => { this.alertService.error(error._body); }
            );
    }

    private loadMembers() {
        this.leagueService.getMembers(this.leagueId)
            .subscribe(
            data => { this.members = data.json(); },
            error => { this.alertService.error(error._body); }
            );
    }

    savechanges() {
        
        this.leagueService.update(this.league)
            .subscribe(
            data => {
                this.leagueService.updateMembers(this.leagueId, this.members)
                    .subscribe(
                        data => { },
                        error => { this.alertService.error(error._body); }
                    );
            },
            error => { this.alertService.error(error._body); }
        );

        
    }


}