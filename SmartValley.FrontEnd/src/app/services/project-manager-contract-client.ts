import {Injectable} from '@angular/core';
import {AuthenticationService} from './authentication-service';
import {Web3Service} from './web3-service';

@Injectable()
export class ProjectManagerContractClient {
  constructor(private authenticationService: AuthenticationService,
              private web3Service: Web3Service) {
  }

  public addProjectAsync(contractAddress: string,
                         abiString: string,
                         projectId: string,
                         name: string): Promise<string> {
    const projectContract = this.web3Service.getContract(abiString, contractAddress);
    const fromAddress = this.authenticationService.getCurrentUser().account;

    return projectContract.addProject(
      projectId.replace(/-/g,  ''),
      name,
      {from: fromAddress});
  }
}
