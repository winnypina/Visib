import {NgModule} from '@angular/core';

import {SharedModule} from '@app/shared/shared.module'

import {TextTranslationRoutingModule} from './textTranslation-routing.module';
import {TextTranslationListComponent} from './textTranslation.list.component';
import {TextTranslationNovoComponent} from './textTranslation.new.component';
import { SmartadminDatatableModule } from '@app/shared/ui/datatable/smartadmin-datatable.module';
import { NgxMaskModule } from 'ngx-mask';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';

@NgModule({
  imports: [
    SharedModule,
    TextTranslationRoutingModule,
    SmartadminDatatableModule,
    NgxDatatableModule,
    NgxMaskModule.forRoot()    
  ],
  declarations: [
    TextTranslationListComponent,
    TextTranslationNovoComponent
  ],
  providers: [],
})
export class TextTranslationModule {

}
