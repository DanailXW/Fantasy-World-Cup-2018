import { Component, OnInit } from '@angular/core';
import { CompetitionService, AlertService } from '../../services/index';

@Component({
    selector: 'groupstandings',
    templateUrl: './groupstandings.component.html'

})
export class GroupStandingsComponent {

    standings: any[] = [];
    groups: string[] = [];

    constructor(private competitionService: CompetitionService, private alertService: AlertService) {

    }

    ngOnInit() {
        this.loadStandings();
    }

    private loadStandings() {
        this.competitionService.getGroupStandings().subscribe(
            data => {
                this.standings = data.sort(
                    (left: any, right: any): number => {
                        if (left.stage.name > right.stage.name) return 1;
                        if (left.stage.name < right.stage.name) return -1;

                        if (left.points < right.points) return 1;
                        if (left.points > right.points) return -1;

                        if ((left.goalsFor - left.goalsAgainst) < (right.goalsFor - right.goalsAgainst)) return 1;
                        if ((left.goalsFor - left.goalsAgainst) > (right.goalsFor - right.goalsAgainst)) return -1;

                        if (left.goalsFor < right.goalsFor) return 1;
                        if (left.goalsFor > right.goalsFor) return -1;

                        return 0;
                    }
                );

                this.groups = this.standings.map(x => x.stage.name).filter((x, i, a) => x && a.indexOf(x) === i);
            },
            error => { this.alertService.error(error._body); });
    }
}