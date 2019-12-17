import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {PrivacyPolicyComponent} from "./privacyPolicy.component";

const routes: Routes = [{
  path: '',
  component: PrivacyPolicyComponent,
  data: {pageTitle: 'Política de privacidade'}
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers: []
})
export class PrivacyPolicyRoutingModule { }
