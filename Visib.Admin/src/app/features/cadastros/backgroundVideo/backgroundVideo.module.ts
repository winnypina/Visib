import {NgModule} from '@angular/core';

import {SharedModule} from '@app/shared/shared.module'

import {BackgroundVideoRoutingModule} from './backgroundVideo-routing.module';
import {BackgroundVideoComponent} from './backgroundVideo.component';
import { SmartadminDatatableModule } from '@app/shared/ui/datatable/smartadmin-datatable.module';


@NgModule({
  imports: [
    SharedModule,
    BackgroundVideoRoutingModule ,
    SmartadminDatatableModule
  ],
  declarations: [
    BackgroundVideoComponent
  ],
  providers: [],
})
export class BackgroundVideoModule {

}
