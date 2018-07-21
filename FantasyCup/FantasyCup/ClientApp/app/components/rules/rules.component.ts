import { Component, OnInit } from '@angular/core'; 
import { ConfigService } from '../../services/config.service';

@Component({
    selector: 'rules',
    templateUrl: './rules.component.html',
    styleUrls: ['../../styles/divs.css']
})
export class RulesComponent {

    groupPhase: any[] = [];
    knockoutPhase: any[] = [];

    constructor(private configService: ConfigService) { }

    ngOnInit() {
        this.loadStages();
    }

    private loadStages() {
        this.configService.getStageTypes()
            .subscribe(data => {
                this.groupPhase = data.filter( (x:any) => x.name == 'Group');
                this.knockoutPhase = data.filter((x: any) => x.name == 'Elimination');
            })
    }

}