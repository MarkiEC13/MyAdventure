import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdventureListComponent } from './adventure-list/adventure-list.component';

const routes: Routes = [
  { path: '', redirectTo: 'adventure-list', pathMatch: 'full' },
  { path: 'adventure-list', component: AdventureListComponent },
  { path: 'adventure/:id', loadChildren: ()=> import('./adventure-details/adventure-details.module').then(m => m.AdventureDetailsModule) }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
