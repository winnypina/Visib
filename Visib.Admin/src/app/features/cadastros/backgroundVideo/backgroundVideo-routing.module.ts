import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {BackgroundVideoComponent} from "./backgroundVideo.component";

const routes: Routes = [{
  path: '',
  component: BackgroundVideoComponent,
  data: {pageTitle: 'Vídeo de abertura'}
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers: []
})
export class BackgroundVideoRoutingModule { }
