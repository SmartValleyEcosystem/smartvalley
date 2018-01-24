export interface Balance {
  ethBalance: number;
  svtBalance: number;
  availableBalance: number;
  wasEtherReceived: boolean;
  canReceiveSvt: boolean;
}
