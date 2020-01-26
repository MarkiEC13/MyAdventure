import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TreeChartNodeComponent } from './tree-chart-node.component';

describe('TreeChartNodeComponent', () => {
  let component: TreeChartNodeComponent;
  let fixture: ComponentFixture<TreeChartNodeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TreeChartNodeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TreeChartNodeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});