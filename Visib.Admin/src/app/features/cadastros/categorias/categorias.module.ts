import {NgModule} from '@angular/core';

import {SharedModule} from '@app/shared/shared.module'

import {CategoriasRoutingModule} from './categorias-routing.module';
import {CategoriasListComponent} from './categorias.list.component';
import {CategoriasNovoComponent} from './categorias.new.component';
import { SmartadminDatatableModule } from '@app/shared/ui/datatable/smartadmin-datatable.module';
import { NgxMaskModule } from 'ngx-mask';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';


@NgModule({
  imports: [
    SharedModule,
    CategoriasRoutingModule,
    SmartadminDatatableModule,
    NgxDatatableModule,
    NgxMaskModule.forRoot(),
  ],
  declarations: [
    CategoriasListComponent,
    CategoriasNovoComponent
  ],
  providers: [],
})
export class CategoriasModule {

}
