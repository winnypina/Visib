import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {UseTermsComponent} from "./useTerms.component";

const routes: Routes = [{
  path: '',
  component: UseTermsComponent,
  data: {pageTitle: 'Termos de uso'}
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers: []
})
export class UseTermsRoutingModule { }
