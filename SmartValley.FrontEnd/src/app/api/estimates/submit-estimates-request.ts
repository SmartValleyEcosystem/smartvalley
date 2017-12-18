import {EstimateCommentRequest} from './estimate-comment-request';

export interface SubmitEstimatesRequest {
  transactionHash: string;

  projectId: number;

  expertAddress: string;

  estimateComments: Array<EstimateCommentRequest>;
}
