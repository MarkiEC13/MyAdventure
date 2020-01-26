import { Component, OnInit } from '@angular/core';
import { AdventureService } from '../../services/adventure.service';
import { ActivatedRoute } from '@angular/router';
import { IItem } from '../item';

@Component({
  selector: 'app-adventure',
  templateUrl: './adventure.component.html',
  styleUrls: ['./adventure.component.scss']
})
export class AdventureComponent implements OnInit {
  adventureId: string;
  ds = {};
  loadFromRootStatus: string = "first";
  loadAllStatus: string = "all";

  constructor(private _Activatedroute: ActivatedRoute, 
    private service: AdventureService) {
  }
  
  ngOnInit() {
    this._Activatedroute.paramMap.subscribe(params => { 
       this.adventureId = params.get('id'); 
       this.getData(this.adventureId, "", this.loadFromRootStatus); 
   });
  }

  onClickNode(selectedNode: IItem) {
    event.preventDefault();
    event.stopPropagation();
    
    selectedNode.cssClass += " select";
    this.service.setSelectedNodes(selectedNode);

    this.service.get(this.adventureId, selectedNode.id)
      .subscribe((data: any[]) => {
        if(!data || !data.length || !data[0].children)
        {
          this.getData(this.adventureId, '', this.loadAllStatus);
        }

        selectedNode.children =  this.service.mapData(data);
      });
  }

  getData(adventureId: string, choiceId: string, status: string = "")
  {
    this.service.get(adventureId, choiceId, status)
      .subscribe((data: any[]) => {
        switch(status)
        {
          case this.loadFromRootStatus:
            this.ds = this.service.mapData(data)[0]
            break;
          case this.loadAllStatus:
            this.ds = this.service.restructureChartData(data);
            break;
          default:
            this.service.flatToTree(data, null, 'parentId', "");
            break;
        }
      });
  }
}
