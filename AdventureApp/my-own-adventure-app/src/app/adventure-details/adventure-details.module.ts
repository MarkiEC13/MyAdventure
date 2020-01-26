import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TreeChartComponent } from './tree-chart/tree-chart.component';
import { TreeChartNodeComponent } from './tree-chart-node/tree-chart-node.component';
import { AdventureDetailsRoutingModule } from './adventure-details-routing.module';
import { AdventureComponent } from './adventure/adventure.component';


@NgModule({
  declarations: [AdventureComponent, TreeChartComponent, TreeChartNodeComponent],
  imports: [
    CommonModule,
    AdventureDetailsRoutingModule
  ],
  exports: [TreeChartComponent, TreeChartNodeComponent]
})
export class AdventureDetailsModule { }
