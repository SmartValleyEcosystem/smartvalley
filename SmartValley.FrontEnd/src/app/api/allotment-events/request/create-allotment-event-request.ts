export interface CreateAllotmentEventRequest {
  name: string;
  tokenContractAddress: string;
  tokenDecimals: number;
  tokenTicker: string;
  projectId: number;
  finishDate?: Date;
}
