import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserDashboardComponent } from './components/user-dashboard/user-dashboard.component';
import { UserRoutingModule } from './user-routing.module';
import { CommonModule as SharedCommonModule } from '../common/common.module';

@NgModule({
  declarations: [
  ],
  imports: [
    UserDashboardComponent,
    CommonModule,
    UserRoutingModule,
    SharedCommonModule
  ]
})
export class UserModule { } 