import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions, Response } from '@angular/http';
import { Router, DefaultUrlSerializer } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { AuthenticationService } from './authentication.service';
import { AppConfig } from '../app.config';

@Injectable()
export class ApiService {
    constructor(private http: Http,
        private authService: AuthenticationService,
        private router: Router,
        private config: AppConfig
    ) {
    }

    http_get(url: string, authenticate: boolean): Observable<any> {            
        return this.http
            .get(((url.indexOf(this.config.apiUrl, 0) > -1) ? '' : this.config.apiUrl) + url, authenticate ? this.jwt() : undefined)
            .map(response => this.extract_data(response))
            .catch(error => this.handleError(error));
        
    }

    http_post(url: string, requestBody: any, authenticate: boolean): Observable<any> {
        return this.http
            .post(((url.indexOf(this.config.apiUrl, 0) > -1) ? '' : this.config.apiUrl) + url, requestBody, authenticate ? this.jwt() : undefined)
            .catch(error => this.handleError_Post(error, requestBody));
    }

    private extract_data(response: Response) {
        let body = response.json();
        return body || [];
    }

    private handleError(error: any) {        
        if (error.status == 401) {
            let auth_header = error.headers.get('www-authenticate');
            if (auth_header && auth_header == 'Bearer error="invalid_token", error_description="The token is expired"') {
                return this.authService.refreshToken()
                    .map(success => this.retry_get(error, success))
                    .mergeMap(val => val)
                    .catch(error_refresh => {
                        return Observable.throw(error_refresh);
                         });
            }
            else
                return Observable.throw(error);                
        }
        else
            return Observable.throw(error);
    }

    private handleError_Post(error: any, requestBody: any) {
        if (error.status == 401) {
            let auth_header = error.headers.get('www-authenticate');
            if (auth_header && auth_header == 'Bearer error="invalid_token", error_description="The token is expired"') {
                return this.authService.refreshToken()
                    .map(success => this.retry_post(error, success, requestBody))
                    .mergeMap(val => val)
                    .catch(error_refresh => {
                        this.router.navigate(['/login']);
                        return Observable.empty();
                    });
            }
            else
                return Observable.throw(error);
        }
        else
            return Observable.throw(error);
    }

    private retry_get(error: any, user: any) {
        if (user && user.token) {
            return this.http_get(error.url, true); //retry
        }
        else {
            this.router.navigate(['/login']);
            return Observable.empty();
        }
    }

    private retry_post(error: any, user: any, body: any) {
        if (user && user.token) {
           
            return this.http_post(error.url, body, true); //retry
        }
        else {
            this.router.navigate(['/login']);
            return Observable.empty();
        }
    }

    private jwt() {
        // create authorization header with jwt token
        let userFromStorage = localStorage.getItem('FantasyUser');
        let currentUser;

        if (userFromStorage)
            currentUser = JSON.parse(userFromStorage);

        if (currentUser && currentUser.token) {
            let headers = new Headers({ 'Authorization': 'Bearer ' + currentUser.token });
            return new RequestOptions({ headers: headers });
        }
    }
}