import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdventureService } from './adventure.service';


@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ],
  providers: [ AdventureService ]
})
export class ServicesModule { }
