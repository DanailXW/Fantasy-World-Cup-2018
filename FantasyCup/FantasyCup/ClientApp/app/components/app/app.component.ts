import { Component, OnInit, ViewContainerRef } from '@angular/core';
import { Router } from '@angular/router';
import { User } from '../../models/index';
import { AuthenticationService } from '../../services/index';
import { Observable } from 'rxjs/Observable';

@Component({
    selector: 'app',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent {
    currentUser: User;
    bLoggedIn$: Observable<boolean>;

    constructor(private authService: AuthenticationService,
        private router: Router
    ) {
        
    }

    ngOnInit() {
        this.bLoggedIn$ = this.authService.isLoggedIn();
    }

    logout(event: Event) {
        this.authService.logout();
        this.router.navigate(['/']);
    }

    navigate(path: string) {
        if (path == 'BetGames')
            this.router.navigate(['bet/games']);
        else if (path == 'BetSpecials')
            this.router.navigate(['bet/competition']);
        else if (path == 'GroupStandings')
            this.router.navigate(['groupstandings']);
        else if (path == 'TopScorers')
            this.router.navigate(['playerstats']);
        else if (path == 'Leagues')
            this.router.navigate(['leagues']);
        else if (path == 'GamesUpcoming')
            this.router.navigate(['games', 'upcoming']);
        else if (path == 'GamesPast')
            this.router.navigate(['games/result']);
        else if (path == 'GamesGroup')
            this.router.navigate(['games/group_phase']);
        else if (path == 'GamesKnockout')
            this.router.navigate(['games/knockout_phase']);
        else if (path == 'Rules')
            this.router.navigate(['rules']);
        else
            this.router.navigate(['/']);
    }
}

