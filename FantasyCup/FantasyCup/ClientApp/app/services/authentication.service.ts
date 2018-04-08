import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

import { AppConfig } from '../app.config';

@Injectable()
export class AuthenticationService {

    constructor(
        private http: Http,
        private config: AppConfig
    ) { }

    login(emailaddress: string, password: string) {
        return this.http.post(this.config.apiUrl + '/api/users/authenticate', { emailaddress: emailaddress, password: password })
            .map((response: Response) => {
                // login successful if there's a jwt token in the response
                let user = response.json();                
                if (user && user.token) {
                    // store user details and jwt token in local storage to keep user logged in between page refreshes
                    localStorage.setItem('FantasyUser', JSON.stringify(user));
                }
            });
    }

    logout() {
        localStorage.removeItem('FantasyUser');
    }
}