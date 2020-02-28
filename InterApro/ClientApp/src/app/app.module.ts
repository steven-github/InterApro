import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { CreateAccountComponent } from './create-account/create-account.component';
import { LogInComponent } from './log-in/log-in.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { BuyerComponent } from './dashboard/buyer/buyer.component';
import { BossComponent } from './dashboard/boss/boss.component';
import { FinancialApproverComponent } from './dashboard/financial-approver/financial-approver.component';
import { ProfileBuyerComponent } from './dashboard/buyer/profile/profile.component';
import { HistoryBuyerComponent } from './dashboard/buyer/history/history.component';
import { RequestsBuyerComponent } from './dashboard/buyer/requests/requests.component';
import { ProfileBossComponent } from './dashboard/boss/profile/profile.component';
import { RequestsBossComponent } from './dashboard/boss/requests/requests.component';
import { ProfileApproverComponent } from './dashboard/financial-approver/profile/profile.component';
import { RequestsApproverComponent } from './dashboard/financial-approver/requests/requests.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    CreateAccountComponent,
    LogInComponent,
    ForgotPasswordComponent,
    BuyerComponent,
    BossComponent,
    FinancialApproverComponent,
    ProfileBuyerComponent,
    HistoryBuyerComponent,
    RequestsBuyerComponent,
    ProfileBossComponent,
    RequestsBossComponent,
    ProfileApproverComponent,
    RequestsApproverComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    ReactiveFormsModule,
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: LogInComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'log-in', component: LogInComponent },
      { path: 'forgot-password', component: ForgotPasswordComponent },
      { path: 'create-account', component: CreateAccountComponent },
      { path: 'dashboard/buyer', component: BuyerComponent },
      { path: 'dashboard/boss', component: BossComponent },
      { path: 'dashboard/financial-approver', component: FinancialApproverComponent },
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
