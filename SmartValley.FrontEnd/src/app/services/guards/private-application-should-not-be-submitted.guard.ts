import {Injectable} from '@angular/core';
import {CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot} from '@angular/router';
import {ProjectApiClient} from '../../api/project/project-api-client';

@Injectable()
export class PrivateApplicationShouldNotBeSubmitted implements CanActivate {
  constructor(private projectApiClient: ProjectApiClient) {
  }

  async canActivate(next: ActivatedRouteSnapshot,
                    state: RouterStateSnapshot): Promise<boolean> {
    const projectId = +next.paramMap.get('id');
    const project = await this.projectApiClient.getProjectSummaryAsync(projectId);
    return (project.isPrivate && !project.isApplicationSubmitted) || !project.isPrivate;
  }
}
