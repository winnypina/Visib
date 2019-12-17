import {ModuleWithProviders} from "@angular/core"
import {Routes, RouterModule} from '@angular/router';


export const routes: Routes = [
  {
    path: '', redirectTo: 'useTerms', pathMatch: 'full'
  },
  {
    path: 'backgroundVideo',
    loadChildren:'./backgroundVideo/backgroundVideo.module#BackgroundVideoModule',

  },
  {
    path: 'useTerms',
    loadChildren:'./useTerms/useTerms.module#UseTermsModule',

  },
  {
    path: 'categorias',
    loadChildren:'./categorias/categorias.module#CategoriasModule',

  },
  {
    path: 'textTranslation',
    loadChildren:'./textTranslation/textTranslation.module#TextTranslationModule',
  },
  {
    path: 'privacyPolicy',
    loadChildren:'./privacyPolicy/privacyPolicy.module#PrivacyPolicyModule',

  },
  {
    path: 'adminUser',
    loadChildren:'./adminUser/adminUser.module#AdminUserModule',

  }
];

export const routing = RouterModule.forChild(routes);
