import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TextTranslationListComponent } from "./textTranslation.list.component";
import { TextTranslationNovoComponent } from "./textTranslation.new.component";

const routes: Routes = [{
    path: '',
    component: TextTranslationListComponent,
    data: { pageTitle: 'Textos do multi-idiomas' },
},
{
    path: 'data',
    component: TextTranslationNovoComponent,
    data: { pageTitle: 'Textos do multi-idiomas - Novo' }
},
{
    path: 'data/:id',
    component: TextTranslationNovoComponent,
    data: {
        pageTitle: 'Textos do multi-idiomas - Editar'
    }
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
    providers: []
})
export class TextTranslationRoutingModule { }
