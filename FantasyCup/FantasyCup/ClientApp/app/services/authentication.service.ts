import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions, Response } from '@angular/http';
import { Router } from '@angular/router';
import { Observable, BehaviorSubject } from 'rxjs';
import 'rxjs/add/operator/map';

import { AppConfig } from '../app.config';

@Injectable()
export class AuthenticationService {

    isLoginSubject = new BehaviorSubject<boolean>(this.hasToken());

    constructor(
        private http: Http,
        private config: AppConfig,
        private router: Router
    ) { }

    isLoggedIn() {
        return this.isLoginSubject.asObservable();
    }

    login(emailaddress: string, password: string) {
        return this.http.post(this.config.apiUrl + '/api/users/authenticate', { emailaddress: emailaddress, password: password })
            .map((response: Response) => {
                // login successful if there's a jwt token in the response
                let user = response.json();                
                if (user && user.token) {
                    // store user details and jwt token in local storage to keep user logged in between page refreshes
                    localStorage.setItem('FantasyUser', JSON.stringify(user));
                    this.isLoginSubject.next(true);
                }
            });
    }

    logout() {
        localStorage.removeItem('FantasyUser');
        this.isLoginSubject.next(false);
    }

    create(user: any) {
        return this.http.post(this.config.apiUrl + '/api/users/register', user)
            .map((response: Response) => {
                // register successful if there's a jwt token in the response
                let user = response.json();
                if (user && user.token) {
                    // store user details and jwt token in local storage to keep user logged in between page refreshes
                    localStorage.setItem('FantasyUser', JSON.stringify(user));
                    this.isLoginSubject.next(true);
                }
            });
    }

    refreshToken() {
        return this.http.get(this.config.apiUrl + '/api/users/refresh', this.jwt_refresh())
            .map((response: Response) => {
                // login successful if there's a jwt token in the response
                let user = response.json();
                if (user && user.token) {
                    // store user details and jwt token in local storage to keep user logged in between page refreshes
                    localStorage.setItem('FantasyUser', JSON.stringify(user));
                }
                return user;
            })
            .catch(error => {
                this.logout();
                this.router.navigate(['login']);
                return Observable.of(null);
            });
            ;
    }

    private hasToken() {
        
        if (typeof window !== 'undefined') {
            let userFromStorage = localStorage.getItem('FantasyUser');
            if (userFromStorage)
                return true;
        }

        return false;
    }

    private jwt_refresh() {
        // create authorization header with jwt token
        let userFromStorage = localStorage.getItem('FantasyUser');
        let currentUser;

        if (userFromStorage)
            currentUser = JSON.parse(userFromStorage);

        if (currentUser && currentUser.refreshToken) {
            let headers = new Headers({ 'Authorization': 'Bearer ' + currentUser.refreshToken });
            return new RequestOptions({ headers: headers });
        }
    }
}