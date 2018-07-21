import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';
import { BrowserModule } from '@angular/platform-browser';

import { NgxPaginationModule } from 'ngx-pagination';
import { ToastrModule, ToastrConfig, ToastrService } from 'ngx-toastr';

import { AppComponent } from './components/app/app.component';
import { AppConfig } from './app.config';

import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { LeaguesComponent, CreateLeagueComponent, JoinLeagueComponent, ManageLeagueComponent, LeagueStandingsComponent } from './components/leagues/index';
import { GameBetsComponent, CompetitionBetsComponent, OthersBetsComponent } from './components/bet/index';
import { PlayerStatsComponent, GamesComponent, GroupStandingsComponent } from './components/competition/index';
import { RulesComponent } from './components/rules/rules.component';

import { AuthenticationService } from './services/authentication.service';
import { ApiService } from './services/api.service';
import { AlertService, UserService, LeagueService, HomeGuard, AccessGuard, ConfigService } from './services/index';
import { BetService } from './services/bet.service';
import { CompetitionService } from './services/competition.service';
import { TeamResolver, PlayerResolver, CompetitionBetsResolver, GameResolver } from './services/resolver.service';

import { EqualValidator } from './directives/validateEqual.directive';


@NgModule({
    declarations: [
        AppComponent,
        LoginComponent,
        RegisterComponent,
        HomeComponent,
        LeaguesComponent,
        CreateLeagueComponent,
        JoinLeagueComponent,
        ManageLeagueComponent,
        LeagueStandingsComponent,
        GameBetsComponent,
        CompetitionBetsComponent,
        OthersBetsComponent,
        PlayerStatsComponent,
        GamesComponent,
        EqualValidator,
        RulesComponent,
        GroupStandingsComponent
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        BrowserModule,
        NgxPaginationModule,
        ToastrModule.forRoot({ positionClass: 'toast-top-center', timeOut: 2000 }),
        RouterModule.forRoot([
            { path: '', component: HomeComponent, canActivate: [HomeGuard] },
            { path: 'login', component: LoginComponent, canActivate: [HomeGuard] },
            { path: 'register', component: RegisterComponent, canActivate: [HomeGuard] },
            { path: 'leagues', component: LeaguesComponent, canActivate: [AccessGuard] },
            { path: 'league/create', component: CreateLeagueComponent, canActivate: [AccessGuard] },
            { path: 'league/join', component: JoinLeagueComponent, canActivate: [AccessGuard] },
            { path: 'league/manage/:id', component: ManageLeagueComponent, canActivate: [AccessGuard] },
            { path: 'league/standings/:id', component: LeagueStandingsComponent, canActivate: [AccessGuard] },
            { path: 'bet/games', component: GameBetsComponent, canActivate: [AccessGuard] },
            { path: 'bet/competition', component: CompetitionBetsComponent, canActivate: [AccessGuard], resolve: { teams: TeamResolver, players: PlayerResolver, bets: CompetitionBetsResolver } },
            { path: 'game/:id/bets', component: OthersBetsComponent, canActivate: [AccessGuard] },
            { path: 'playerstats', component: PlayerStatsComponent },
            {
                path: 'games', component: GamesComponent, resolve: { games: GameResolver },
                children: [
                    { path: 'upcoming', component: GamesComponent},
                    { path: 'result', component: GamesComponent },
                    { path: 'group_phase', component: GamesComponent },
                    { path: 'knockout_phase', component: GamesComponent }
                ]
            },
            { path: 'rules', component: RulesComponent },
            { path: 'groupstandings', component: GroupStandingsComponent}
        ])
    ],
    providers: [
        AppConfig,
        AlertService,
        AuthenticationService,
        ApiService,
        UserService,
        LeagueService,
        BetService,
        CompetitionService,
        HomeGuard,
        AccessGuard,
        TeamResolver,
        PlayerResolver,
        CompetitionBetsResolver,
        GameResolver,
        ConfigService
    ]
})
export class AppModuleShared {
}
