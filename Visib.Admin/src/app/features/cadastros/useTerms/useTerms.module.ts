import {NgModule} from '@angular/core';

import {SharedModule} from '@app/shared/shared.module'

import {UseTermsRoutingModule} from './useTerms-routing.module';
import {UseTermsComponent} from './useTerms.component';
import { SmartadminDatatableModule } from '@app/shared/ui/datatable/smartadmin-datatable.module';


@NgModule({
  imports: [
    SharedModule,
    UseTermsRoutingModule ,
    SmartadminDatatableModule   
  ],
  declarations: [
    UseTermsComponent
  ],
  providers: [],
})
export class UseTermsModule {

}
