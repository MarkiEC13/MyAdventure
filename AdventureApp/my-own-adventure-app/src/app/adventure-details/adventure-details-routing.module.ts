import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AdventureComponent } from './adventure/adventure.component';


const routes: Routes = [
  {
    path: '',
    component: AdventureComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdventureDetailsRoutingModule { }