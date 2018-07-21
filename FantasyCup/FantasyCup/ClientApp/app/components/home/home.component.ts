import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { AuthenticationService } from '../../services/index';

@Component({
    selector: 'home',
    templateUrl: './home.component.html'
})
export class HomeComponent {

    bLoggedIn$: Observable<boolean>;

    constructor(private authService: AuthenticationService, private router: Router) {
        
    }

    ngOnInit() {        
        this.bLoggedIn$ = this.authService.isLoggedIn();
    }

    register() {
        this.router.navigate(['register']);
    }
}
