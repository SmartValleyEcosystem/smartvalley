export interface EditAllotmentRequest {
  id: number;
  eventName: string;
  tokenAddress: string;
  ticker: string;
  tokenDecimals: number;
  finishDate?: Date;
}
