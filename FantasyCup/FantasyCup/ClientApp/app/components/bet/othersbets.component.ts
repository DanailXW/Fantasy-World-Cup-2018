import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { BetService, AlertService } from '../../services/index';
import { GameBet } from '../../models/index';

@Component({
    selector: 'othersbets',
    templateUrl: './othersbets.component.html',
    styleUrls: ['../../styles/tables.css']
})
export class OthersBetsComponent {

    bets: any[] = [];
    gameId: string;

    constructor(private betService: BetService,
        private alertService: AlertService,
        private router: Router,
        private route: ActivatedRoute
    ) { }

    ngOnInit() {
        this.gameId = this.route.snapshot.paramMap.get('id') || '';
        this.loadBets(this.gameId);
    }

    private loadBets(gameId: string) {
        this.betService.getOthersBets(gameId).subscribe(
            data => { this.bets = data; },
            error => { this.alertService.error(error._body); }
        );        
    }
}