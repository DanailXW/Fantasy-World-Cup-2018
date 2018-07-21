import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, UrlSegment } from '@angular/router';
import { CompetitionService, AlertService } from '../../services/index';

@Component({
    selector: 'games',
    templateUrl: 'games.component.html'
})
export class GamesComponent {

    games: any[] = [];
    filtered_games: any[] = [];
    filter: string;

    constructor(private competitionService: CompetitionService,
        private alertService: AlertService,
        private route: ActivatedRoute
    ) {
        
    }

    ngOnInit() {    

        this.games = this.route.snapshot.data['games'];
        this.filterGames();

        this.route.url.subscribe(url => {
            this.filterGames();
        });
    }

    private loadGames() {
        this.competitionService.getGames().subscribe(
            data => {
                this.games = data.sort(this.sort);
                this.filtered_games = data.sort(this.sort);
            },
            error => { this.alertService.error(error._body); }
        )
    }

    private filterGames() {
        let filter = (this.route.snapshot.firstChild && this.route.snapshot.firstChild.url[0].path) || 'result';

        if (filter == 'upcoming')
            this.filtered_games = this.games.filter(x => x.state != 'Finished').sort(this.sort);
        else if (filter == 'result')
            this.filtered_games = this.games.filter(x => x.state == 'Finished').sort(this.sort);
        else if (filter == 'group_phase')
            this.filtered_games = this.games.filter(x => x.stageType == 'Group').sort(this.sort);
        else if (filter == 'knockout_phase')
            this.filtered_games = this.games.filter(x => x.stageType == 'Elimination').sort(this.sort);
    }

    private sort(left: any, right: any): number {
        if (left.startDate > right.startDate) return 1;
        if (left.startDate < right.startDate) return -1;
        return 0;
    }
}
