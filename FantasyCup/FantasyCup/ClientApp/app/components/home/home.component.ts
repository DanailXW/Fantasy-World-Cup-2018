import { Component } from '@angular/core';
import { User } from '../../models/index';
import { AuthenticationService } from '../../services/index';

@Component({
    selector: 'home',
    templateUrl: './home.component.html'
})
export class HomeComponent {
    currentUser: User;

    constructor(private authService: AuthenticationService) {
        let userFromStorage = localStorage.getItem('FantasyUser');
        if (userFromStorage)
            this.currentUser = JSON.parse(userFromStorage);
    }

    logout(event: Event) {
        this.authService.logout();
    }
}
