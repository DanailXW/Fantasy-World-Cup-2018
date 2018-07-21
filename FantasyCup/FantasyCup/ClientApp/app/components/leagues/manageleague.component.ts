import { Component, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { LeagueService, AlertService } from '../../services/index';
import { League, LeagueMember } from '../../models/index';

@Component({
    selector: 'manageleague',
    templateUrl: './manageleague.component.html'
})
export class ManageLeagueComponent {
    @ViewChild('f') form: any;
    members: LeagueMember[] = [];
    leagueId: string;
    league: any = {};

    constructor(private leagueService: LeagueService,
        private alertService: AlertService,
        private route: ActivatedRoute,
        private router: Router
    ) { }

    ngOnInit() {
        this.leagueId = this.route.snapshot.paramMap.get('id') || '';
        this.loadLeague();
        //this.loadMembers();
    }

    private loadLeague() {
        this.leagueService.get(this.leagueId)
            .subscribe(
            data => { this.league = data; this.loadMembers(); },
                error => { this.alertService.error(error._body); }
            );
    }

    private loadMembers() {
        this.leagueService.getMembers(this.leagueId)
            .subscribe(
            data => { this.members = data; },
            error => { this.alertService.error(error._body); }
            );
    }

    savechanges() {
        
        this.leagueService.update(this.league)
            .subscribe(
            data => {
                this.leagueService.updateMembers(this.leagueId, this.members)
                    .subscribe(
                    data => { this.alertService.success('League updated successfully!'); this.form.form.markAsPristine(); },
                        error => { this.alertService.error(error._body); }
                    );
            },
            error => { this.alertService.error(error._body); }
        );        
    }

    delete() {
        this.leagueService.delete(this.league)
            .subscribe(
            data => {
                this.alertService.success('The league was deleted!');
                this.router.navigate(['leagues']);
            },
                error => { this.alertService.error(error._body); }
            );
    }


}