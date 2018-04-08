import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { AppConfig } from './app.config';

import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { FetchDataComponent } from './components/fetchdata/fetchdata.component';
import { CounterComponent } from './components/counter/counter.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { LeaguesComponent, CreateLeagueComponent, JoinLeagueComponent, ManageLeagueComponent } from './components/leagues/index';

import { AuthenticationService, AlertService, UserService, LeagueService } from './services/index';

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        CounterComponent,
        FetchDataComponent,
        LoginComponent,
        RegisterComponent,
        HomeComponent,
        LeaguesComponent,
        CreateLeagueComponent,
        JoinLeagueComponent,
        ManageLeagueComponent
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        RouterModule.forRoot([
            //{ path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            { path: 'counter', component: CounterComponent },
            { path: 'fetch-data', component: FetchDataComponent },
            { path: 'login', component: LoginComponent },
            { path: 'register', component: RegisterComponent },
            { path: 'leagues', component: LeaguesComponent },
            { path: 'league/create', component: CreateLeagueComponent },
            { path: 'league/join', component: JoinLeagueComponent },
            { path: 'league/manage/:id', component: ManageLeagueComponent }
            //{ path: '**', redirectTo: 'home' }
        ])
    ],
    providers: [
        AppConfig,
        AlertService,
        AuthenticationService,
        UserService,
        LeagueService
    ]
})
export class AppModuleShared {
}
