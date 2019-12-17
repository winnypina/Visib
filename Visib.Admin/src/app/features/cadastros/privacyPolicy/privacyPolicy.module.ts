import {NgModule} from '@angular/core';

import {SharedModule} from '@app/shared/shared.module'

import {PrivacyPolicyRoutingModule} from './privacyPolicy-routing.module';
import {PrivacyPolicyComponent} from './privacyPolicy.component';
import { SmartadminDatatableModule } from '@app/shared/ui/datatable/smartadmin-datatable.module';


@NgModule({
  imports: [
    SharedModule,
    PrivacyPolicyRoutingModule ,
    SmartadminDatatableModule   
  ],
  declarations: [
    PrivacyPolicyComponent
  ],
  providers: [],
})
export class PrivacyPolicyModule {

}
