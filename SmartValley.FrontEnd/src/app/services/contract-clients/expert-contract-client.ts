import {Injectable} from '@angular/core';
import {Web3Service} from '../web3-service';
import {ContractApiClient} from '../../api/contract/contract-api-client';
import {ContractClient} from './contract-client';
import {UserContext} from '../authentication/user-context';

@Injectable()
export class ExpertContractClient implements ContractClient {

    public abi: string;
    public address: string;

    constructor(private web3Service: Web3Service,
                private contractClient: ContractApiClient,
                private userContext: UserContext) {
    }

    public async initializeAsync(): Promise<void> {
        const tokenContract = await this.contractClient.getExpertRegistryContractAsync();
        this.abi = tokenContract.abi;
        this.address = tokenContract.address;
    }

    public applyAsync(expertiseAreas: number[]): Promise<string> {
        const contract = this.web3Service.getContract(this.abi, this.address);
        const fromAddress = this.userContext.getCurrentUser().account;

        return contract.apply(expertiseAreas, {from: fromAddress});
    }

    public addSync(expertAddress: string, expertiseAreas: number[]): Promise<string> {
        const contract = this.web3Service.getContract(this.abi, this.address);
        const fromAddress = this.userContext.getCurrentUser().account;

        return contract.add(expertAddress, expertiseAreas, {from: fromAddress});
    }
}
