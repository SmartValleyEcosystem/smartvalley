export interface ContractClient {
  abi: string;
  address: string;
  initializeAsync(): Promise<void>;
}
