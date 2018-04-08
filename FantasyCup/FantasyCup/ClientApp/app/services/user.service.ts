import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions, Response } from '@angular/http';

import { AppConfig } from '../app.config';
import { User } from '../models/index';

@Injectable()
export class UserService {
    constructor(
        private http: Http,
        private config: AppConfig
    ) { }

    create(user: User) {
        return this.http.post(this.config.apiUrl + '/api/users/register', user)
            .map((response: Response) => {
                // register successful if there's a jwt token in the response
                let user = response.json();
                if (user && user.token) {
                    // store user details and jwt token in local storage to keep user logged in between page refreshes
                    localStorage.setItem('FantasyUser', JSON.stringify(user));
                }
            });
    }
}