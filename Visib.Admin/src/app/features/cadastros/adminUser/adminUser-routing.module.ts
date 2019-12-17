import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdminUserListComponent } from "./adminUser.list.component";
import { AdminUserNovoComponent } from "./adminUser.new.component";

const routes: Routes = [{
    path: '',
    component: AdminUserListComponent,
    data: { pageTitle: 'Cadastro de usuário administrativo' },
},
{
    path: 'data',
    component: AdminUserNovoComponent,
    data: { pageTitle: 'Cadastro de usuário administrativo - Novo' }
},
{
    path: 'data/:id',
    component: AdminUserNovoComponent,
    data: {
        pageTitle: 'Cadastro de usuário administrativo - Editar'
    }
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
    providers: []
})
export class AdminUserRoutingModule { }
