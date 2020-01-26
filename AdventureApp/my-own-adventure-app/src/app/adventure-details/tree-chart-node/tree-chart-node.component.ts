import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { IItem } from '../item';

@Component({
  selector: 'app-tree-chart-node',
  templateUrl: './tree-chart-node.component.html',
  styleUrls: ['./tree-chart-node.component.scss']
})
export class TreeChartNodeComponent implements OnInit {
  ngOnInit(): void {
  }

  @Input() item: IItem;
  @Input() hasParent = false;

  @Output() itemClick = new EventEmitter<IItem>();

  

}
