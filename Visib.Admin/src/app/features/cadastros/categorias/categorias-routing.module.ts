import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {CategoriasListComponent} from "./categorias.list.component";
import {CategoriasNovoComponent} from "./categorias.new.component";

const routes: Routes = [{
  path: '',
  component: CategoriasListComponent,
  data: {pageTitle: 'Categorias'},
},
{
  path: 'data',
  component: CategoriasNovoComponent,
  data: {pageTitle: 'Categorias - Novo'}
},
{ path: 'data/:id', component: CategoriasNovoComponent, data: {pageTitle: 'Categorias - Editar'} }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers: []
})
export class CategoriasRoutingModule { }
