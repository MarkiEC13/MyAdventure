import { Component, OnInit, Input, Output, HostBinding, EventEmitter } from '@angular/core';
import { IItem } from '../item';

@Component({
  selector: 'app-tree-chart',
  templateUrl: './tree-chart.component.html',
  styleUrls: ['./tree-chart.component.scss']
})
export class TreeChartComponent {

  @Input() item: IItem;
  @Input() hasParent = false;

  @Output() itemClick = new EventEmitter<IItem>();
}
