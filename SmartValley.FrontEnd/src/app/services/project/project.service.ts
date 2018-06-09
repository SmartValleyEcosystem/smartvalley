import {EventEmitter, Injectable} from '@angular/core';
import {ProjectApiClient} from '../../api/project/project-api-client';
import {CreateProjectRequest} from '../../api/project/create-project-request';
import {UpdateProjectRequest} from '../../api/project/update-project-request';
import {AddProjectTeamMemberPhotoRequest} from '../../api/project/add-project-team-member-photo-request';
import {NotificationsService} from 'angular2-notifications';
import {ErrorCode} from '../../shared/error-code.enum';
import {TranslateService} from '@ngx-translate/core';
import {ProjectAboutResponse} from '../../api/project/project-about-response';
import {AddProjectImageRequest} from '../../api/project/add-project-image-request';
import {ScoringStatus} from '../scoring-status.enum';
import {ScoringStartTransactionStatus} from '../../api/project/scoring-start-transaction.status';
import {environment} from '../../../environments/environment';

@Injectable()
export class ProjectService {

  public projectsCreated: EventEmitter<any> = new EventEmitter<any>();
  public projectsDeleted: EventEmitter<any> = new EventEmitter<any>();

  constructor(private projectApiClient: ProjectApiClient,
              private notificationService: NotificationsService,
              private translateService: TranslateService) {
  }

  public async createAsync(request: CreateProjectRequest): Promise<ProjectAboutResponse> {
    const response = await this.projectApiClient.createAsync(request);
    this.projectsCreated.emit();
    return response;
  }

  public getScoringStatus(status: ScoringStatus,
                          scoringStartTransactionStatus: ScoringStartTransactionStatus,
                          isApplicationSubmitted: boolean): ScoringStatus {
    if (status !== ScoringStatus.FillingApplication) {
      return status;
    }
    if (scoringStartTransactionStatus === ScoringStartTransactionStatus.NotSubmitted) {
      return isApplicationSubmitted ? ScoringStatus.ReadyForPayment : ScoringStatus.FillingApplication;
    }

    if (scoringStartTransactionStatus === ScoringStartTransactionStatus.InProgress) {
      return ScoringStatus.PaymentInProcess;
    }

    if (scoringStartTransactionStatus === ScoringStartTransactionStatus.Failed) {
      return ScoringStatus.PaymentFailed;
    }
    return status;
  }

  public getTransactionUrl(transactionHash: string) {
    return `${environment.etherscan_host}/tx/${transactionHash}`;
  }

  public async updateAsync(request: UpdateProjectRequest): Promise<ProjectAboutResponse> {
    return await this.projectApiClient.updateAsync(request);
  }

  public async uploadProjectImageAsync(projectId: number, image: Blob): Promise<void> {
    const input = new FormData();
    input.append('id', projectId.toString());
    input.append('image', image);
    try {
      await this.projectApiClient.uploadProjectImageAsync(<AddProjectImageRequest>{body: input});
    } catch (e) {
      switch (e.error.errorCode) {
        case ErrorCode.InvalidFileUploaded:
          this.notificationService.error(
            this.translateService.instant('Common.UploadPhotoErrorTitle'),
            this.translateService.instant('Common.TryAgain')
          );
      }
    }
  }

  public async uploadTeamMemberPhotoAsync(projectId: number, memberId: number, photo: Blob): Promise<void> {
    const input = new FormData();
    input.append('projectId', projectId.toString());
    input.append('projectTeamMemberId', memberId.toString());
    input.append('photo', photo);
    try {
      await this.projectApiClient.uploadTeamMemberPhotoAsync(<AddProjectTeamMemberPhotoRequest>{body: input});
    } catch (e) {
      switch (e.error.errorCode) {
        case ErrorCode.InvalidFileUploaded:
          this.notificationService.error(
            this.translateService.instant('Common.UploadPhotoErrorTitle'),
            this.translateService.instant('Common.TryAgain')
          );
      }
    }
  }

  public async deleteAsync(projectId: number): Promise<void> {
    await this.projectApiClient.deleteAsync(projectId);
    this.projectsDeleted.emit();
  }
}
