import { Component } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { UserService, AlertService } from '../../services/index';

@Component({
    selector: 'register',
    templateUrl: './register.component.html'
})
export class RegisterComponent {
    model: any = {};
    returnUrl: string;

    constructor(
        private userService: UserService,
        private alertService: AlertService,
        private router: Router,
        private route: ActivatedRoute,
    ) { }

    ngOnInit() {
        this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
    }

    register() {
        this.userService.create(this.model)
            .subscribe(
                data => {
                    this.router.navigate([this.returnUrl]);
                },
                error => {
                    this.alertService.error(error._body);
                }
            )
    }
}