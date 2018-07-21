import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class AlertService {

    constructor(private toastr: ToastrService) { }

    success(message: string) {
        this.toastr.success(message);
    }
    error(message: string) {
        this.toastr.error(message);
    }
}