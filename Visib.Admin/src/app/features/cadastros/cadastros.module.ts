import {NgModule} from '@angular/core';

import {routing} from './cadastros.routing';
import { SharedModule } from '@app/shared/shared.module';


@NgModule({
  imports: [
    SharedModule,
    routing,
  ],
  declarations: [],
  providers: [],
})
export class CadastrosModule {

}
