import {EventEmitter, Injectable} from '@angular/core';
import {ProjectApiClient} from '../../api/project/project-api-client';
import {CreateProjectRequest} from '../../api/project/create-project-request';
import {UpdateProjectRequest} from '../../api/project/update-project-request';

@Injectable()
export class ProjectService {

  public projectsCreated: EventEmitter<any> = new EventEmitter<any>();
  public projectsDeleted: EventEmitter<any> = new EventEmitter<any>();

  constructor(private projectApiClient: ProjectApiClient) {
  }

  public async createAsync(request: CreateProjectRequest): Promise<void> {
    await this.projectApiClient.createAsync(request);
    this.projectsCreated.emit();
  }

  public async updateAsync(request: UpdateProjectRequest): Promise<void> {
    await this.projectApiClient.updateAsync(request);
  }

  public async deleteAsync(projectId: number): Promise<void> {
    await this.projectApiClient.deleteAsync(projectId);
    this.projectsDeleted.emit();
  }
}
