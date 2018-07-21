import { Injectable } from '@angular/core';
import { ApiService } from './api.service';

@Injectable()
export class ConfigService {
    constructor(private api: ApiService) { }

    getStageTypes() {
        return this.api.http_get('/api/config/stagetype', false);
    }
}