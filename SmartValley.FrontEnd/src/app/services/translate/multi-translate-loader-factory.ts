import {MultiTranslateLoader} from './multi-translate-loader';

export function multiTranslateLoaderFactory() {
  return new MultiTranslateLoader([
    'services/criteria/criteria',
    'services/authentication/authentication-service',
    'services/balance/balance-service',
    'services/translate/common',
    'services/common/countries',
    'services/common/categories',
    'services/common/boolean',
    'services/common/stages',
    'services/common/offerStatuses',
    'components/header/header.component',
    'components/image-uploader/image-uploader.component',
    'components/create-project/create-project.component',
    'components/landing/landing.component',
    'components/admin-panel/admin-panel.component',
    'components/admin-panel/admin-users-list/admin-users-list.component',
    'components/admin-panel/admin-expert-applications-list/admin-expert-applications-list.component',
    'components/admin-panel/admin-expert-application/admin-expert-application.component',
    'components/admin-panel/admin-scoring-projects/admin-scoring-project.component',
    'components/admin-panel/admin-projects-list/admin-projects.component',
    'components/admin-panel/admin-feedbacks/admin-feedbacks.component',
    'components/admin-panel/admin-subscriptions/admin-subscriptions.component',
    'components/expert-status/expert-status.component',
    'components/register-expert/register-expert.component',
    'components/footer/footer.component',
    'components/common/transaction-awaiting-modal/transaction-awaiting-modal.component',
    'components/common/receive-ether-modal/receive-ether-modal.component',
    'components/account/account.component',
    'components/common/create-new-expert-modal/create-new-expert-modal.component',
    'components/admin-panel/admin-experts-list/admin-expert-list.component',
    'components/common/edit-expert-modal/edit-expert-modal.component',
    'components/common/delete-project-modal/delete-project-modal.component',
    'components/project-list/project-list.component',
    'components/scoring-list/scoring-list.component',
    'components/search-with-autocomplete/search-with-autocomplete.component',
    'components/authentication/register/register.component',
    'components/authentication/register-confirm/register-confirm.component',
    'components/select/select.component',
    'components/scoring/offer-details/offer-details.component',
    'components/metamask-howto/metamask-howto.component',
    'components/edit-scoring-application/edit-scoring-application.component',
    'components/common/confirm-email/confirm-email.component',
    'components/project/project.component',
    'components/project-info/project-info.component',
    'components/project/project-about/project-about.component',
    'components/common/waiting-modal/waiting-modal.component',
    'components/common/confirm-email/confirm-email.component',
    'components/project/scoring-application/scoring-application.component',
    'components/expert-scoring/expert-scoring.component',
    'components/common/waiting-modal/waiting-modal.component',
    'components/scoring/scoring-payment/scoring-payment.component',
    'components/project/scoring-report/scoring-report.component',
    'components/common/subscribe-modal/subscribe-modal.component',
    'components/common/feedback-modal/feedback-modal.component'
  ]);
}
