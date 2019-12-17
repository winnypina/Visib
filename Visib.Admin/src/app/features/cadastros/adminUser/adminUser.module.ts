import {NgModule} from '@angular/core';

import {SharedModule} from '@app/shared/shared.module'

import {AdminUserRoutingModule} from './adminUser-routing.module';
import {AdminUserListComponent} from './adminUser.list.component';
import {AdminUserNovoComponent} from './adminUser.new.component';
import { SmartadminDatatableModule } from '@app/shared/ui/datatable/smartadmin-datatable.module';
import { NgxMaskModule } from 'ngx-mask';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';

@NgModule({
  imports: [
    SharedModule,
    AdminUserRoutingModule,
    SmartadminDatatableModule,
    NgxDatatableModule,
    NgxMaskModule.forRoot()    
  ],
  declarations: [
    AdminUserListComponent,
    AdminUserNovoComponent
  ],
  providers: [],
})
export class AdminUserModule {

}
