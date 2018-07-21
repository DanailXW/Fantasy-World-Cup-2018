import { Component } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthenticationService, AlertService } from '../../services/index';

@Component({
    selector: 'register',
    templateUrl: './register.component.html',
    styleUrls: ['../../styles/forms.css']
})
export class RegisterComponent {
    model: any = {};
    returnUrl: string;

    constructor(
        private authService: AuthenticationService,
        private alertService: AlertService,
        private router: Router,
        private route: ActivatedRoute,
    ) { }

    ngOnInit() {
        this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
    }

    register() {
        this.authService.create(this.model)
            .subscribe(
            data => {
                this.alertService.success('Successful registration. Good luck!');
                    this.router.navigate([this.returnUrl]);
                },
                error => {
                    this.alertService.error('Registration was not successful');
                }
            )
    }
}