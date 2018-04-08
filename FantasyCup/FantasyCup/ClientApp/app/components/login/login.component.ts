import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthenticationService, AlertService } from '../../services/index';

@Component({
    selector: 'login',
    templateUrl: './login.component.html'
})
export class LoginComponent {
    model: any = {};
    returnUrl: string;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private authenticationService: AuthenticationService,
        private alertService: AlertService
    ) { }

    ngOnInit() {
        this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
        console.log(this.returnUrl);
    }

    login() {
        this.authenticationService.login(this.model.emailaddress, this.model.password)
            .subscribe(
            data => {
                this.router.navigate(['home']);
            },
            error => {
                this.alertService.error(error._body);
            }
            );
    }
}