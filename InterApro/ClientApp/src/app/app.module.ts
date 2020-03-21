import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { UserService } from './_services/user.service';
import { AuthGuardService } from './_services/auth-guard.service';
import { CommonModule } from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
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
import { ProfileFinancialApproverComponent } from './dashboard/financial-approver/profile/profile.component';
import { RequestsFinancialApproverComponent } from './dashboard/financial-approver/requests/requests.component';
import { AdminComponent } from './dashboard/admin/admin.component';
import { AdminUsersComponent } from './dashboard/admin/users/users.component';
import { AdminCreateAccountComponent } from './dashboard/admin/create-account/create-account.component';


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
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
    ProfileFinancialApproverComponent,
    RequestsFinancialApproverComponent,
    AdminComponent,
    AdminUsersComponent,
    AdminCreateAccountComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    ReactiveFormsModule,
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'log-in', component: LogInComponent },
      { path: 'forgot-password', component: ForgotPasswordComponent },
      { path: 'create-account', component: AdminCreateAccountComponent, canActivate: [AuthGuardService] },
      { path: 'dashboard/admin', component: AdminComponent, canActivate: [AuthGuardService] },
      { path: 'dashboard/buyer', component: BuyerComponent, canActivate: [AuthGuardService] },
      { path: 'dashboard/boss', component: BossComponent, canActivate: [AuthGuardService] },
      { path: 'dashboard/financial-approver', component: FinancialApproverComponent, canActivate: [AuthGuardService] },
    ]),
    CommonModule,
    BrowserAnimationsModule, // required animations module
    ToastrModule.forRoot() // ToastrModule added
  ],
  providers: [UserService, AuthGuardService],
  bootstrap: [AppComponent]
})
export class AppModule { }
