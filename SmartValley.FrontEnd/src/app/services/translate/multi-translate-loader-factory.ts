import {MultiTranslateLoader} from './multi-translate-loader';

export function multiTranslateLoaderFactory() {
  return new MultiTranslateLoader([
    'services/questions/questions',
    'services/authentication/authentication-service',
    'services/balance/balance-service',
    'services/translate/common',
    'services/voting/voting-service',
    'components/header/header.component',
    'components/application/application.component',
    'components/my-projects/my-projects.component',
    'components/landing/landing.component',
    'components/voting/voting.component',
    'components/admin-panel/admin-panel.component',
    'components/become-expert/become-expert.component',
    'components/scoring/scoring.component',
    'components/register-expert/register-expert.component',
    'components/estimate/estimate.component',
    'components/report/report.component',
    'components/voting-card/voting-card.component',
    'components/report/questions/questions.component',
    'components/footer/footer.component',
    'components/common/project-card/project-card.component',
    'components/common/transaction-awaiting-modal/transaction-awaiting-modal.component',
    'components/common/receive-svt-modal/receive-svt-modal.component',
    'components/common/receive-ether-modal/receive-ether-modal.component',
    'components/common/project-information/project-information.component',
    'components/account/account.component',
    'components/common/svt-withdrawal-confirmation-modal/svt-withdrawal-confirmation-modal.component',
    'components/common/free-scoring-confirmation-modal/free-scoring-confirmation-modal.component',
    'components/common/vote-modal/vote-modal.component',
    'components/common/register-modal/register-modal.component',
    'components/common/confirm-email/confirm-email.component',
    'components/common/confirm-email/confirm-email-modal.component',
    'components/completed-voting/completed-voting.component'
  ]);
}
