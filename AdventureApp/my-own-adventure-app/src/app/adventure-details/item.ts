export interface IItem {
    id: string;
    parentId: string;
    value: string;
    cssClass: string;
    children: IItem[];
  }

  export class Item implements IItem {
    id: string;
    parentId: string;
    value: string;
    cssClass: string;
    children: IItem[];
  }