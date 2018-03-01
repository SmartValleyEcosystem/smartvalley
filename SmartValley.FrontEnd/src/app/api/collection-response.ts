export interface CollectionResponse<TItem> {
  items: Array<TItem>;
  totalCount?: number;
}
