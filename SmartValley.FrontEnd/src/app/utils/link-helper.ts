export class LinkHelper {
    public getEtherscanLink(hash: string): string {
        return 'https://rinkeby.etherscan.io/tx/' + hash;
    }
}
