import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from './../../environments/environment';
import { IItem } from '../adventure-details/item';

@Injectable({
  providedIn: 'root'
})
export class AdventureService {

  selectedChoices: string[] = [];

  constructor(private http: HttpClient) { }

  get(adventureId: string, choicesId: string, status: string = "")
  {
    return this.http
      .get(`${environment.apiBaseUrl.replace(/\/$/, "")}/adventures/${adventureId}/choices/${choicesId}?status=${status}`);
  }

  flatToTree = (items, id = null, link = 'parentId', cssClass) =>
    items
      .filter(item => item[link] === id)    
      .map(item => ({ id: item.id, value: item.value, cssClass: cssClass, children: this.flatToTree(items, item.id, 'parentId', 'leaf') }));

  mapData(items: any[], cssClass = "") {
    return (items) ?
     items
      .map(item => ({ id: item.id, value: item.value, cssClass: cssClass, children: this.mapData(item.children, 'leaf') })) :
      [];
  }

  setSelectedNodes(node: IItem)
  {
    if(!this.selectedChoices.includes(node.id))
      this.selectedChoices.push(node.id)
  }

  restructureChartData(data: IItem[])
  {
    if(!data) return;
  
    data[0].children = this.setChildren(data, data[0]);
    return data[0];
  }

  private setChildren(data:IItem[], currentItem: IItem): IItem[]
  {
    if(!currentItem || !currentItem.children) return;

    return currentItem.children.map((child) => ({ 
      ...child,
      cssClass: this.selectedChoices.includes(child.id) ? 'leaf select' : 'leaf',
      children: (child.children) ? 
                    child.children : 
                    data.filter(i => i.parentId == child.id).map<IItem>(grandChildren => ({
                      ...grandChildren,
                        children: this.setChildren(data, grandChildren)
                    }))
     }));
  }
}
