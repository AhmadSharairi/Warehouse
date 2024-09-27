export interface Item {
  id: number;
  name: string;
  skuCode?: string;
  quantity: number;
  costPrice: number;
  msrpPrice?: number;
}
