import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { AuthenticationService } from './index';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/take';
import 'rxjs/add/observable/of';

@Injectable()
export class HomeGuard implements CanActivate{
    constructor(private authService: AuthenticationService,
        private router: Router) {
    }

    canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
        return this.authService.isLoggedIn().map(
            val => {
                if (val) {
                    this.router.navigate(['bet/games']);
                    return false;
                }
                return true;
            }
        )
    }
}

@Injectable()
export class AccessGuard implements CanActivate {
    constructor(private authService: AuthenticationService,
        private router: Router) {
    }

    canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
        return this.authService.isLoggedIn().map(
            val => {
                if (!val) {
                    this.router.navigate(['/']);
                    return false;
                }
                return true;
            }
        )
    }
}
