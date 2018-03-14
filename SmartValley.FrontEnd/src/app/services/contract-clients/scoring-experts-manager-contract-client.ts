import {Injectable} from '@angular/core';
import {Web3Service} from '../web3-service';
import {ContractApiClient} from '../../api/contract/contract-api-client';
import {ContractClient} from './contract-client';
import {UserContext} from '../authentication/user-context';

@Injectable()
export class ScoringExpertsManagerContractClient implements ContractClient {

    public abi: string;
    public address: string;

    constructor(private userContext: UserContext,
                private web3Service: Web3Service,
                private contractClient: ContractApiClient) {
    }

    public async initializeAsync(): Promise<void> {
        const scoringExpertsManagerContract = await this.contractClient.getScoringExpertsManagerContractAsync();
        this.abi = scoringExpertsManagerContract.abi;
        this.address = scoringExpertsManagerContract.address;
    }

    public async selectMissingExpertsAsync(projectId: string): Promise<string> {
        const contract = this.web3Service.getContract(this.abi, this.address);
        const fromAddress = this.userContext.getCurrentUser().account;

        return contract.selectMissingExperts(
            projectId.replace(/-/g, ''),
            {from: fromAddress});
    }

    public async setExpertsAsync(projectId: string,
                          areas: Array<number>,
                          experts: Array<string>): Promise<string> {
        const contract = this.web3Service.getContract(this.abi, this.address);
        const fromAddress = this.userContext.getCurrentUser().account;

        return contract.setExperts(
            projectId.replace(/-/g, ''),
            areas,
            experts,
            {from: fromAddress});
    }

    public async acceptOfferAsync(projectId: string,
                 area: number): Promise<string> {
        const contract = this.web3Service.getContract(this.abi, this.address);
        const fromAddress = this.userContext.getCurrentUser().account;

        return contract.accept(
            projectId.replace(/-/g, ''),
            area);
    }

    public async rejectOfferAsync(projectId: string,
                      area: number): Promise<string> {
        const contract = this.web3Service.getContract(this.abi, this.address);
        const fromAddress = this.userContext.getCurrentUser().account;

        return contract.reject(
            projectId.replace(/-/g, ''),
            area);
    }
}
