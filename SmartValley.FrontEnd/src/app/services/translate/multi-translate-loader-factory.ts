import {MultiTranslateLoader} from './multi-translate-loader';

export function multiTranslateLoaderFactory() {
  return new MultiTranslateLoader([
    'services/questions/questions',
    'services/authentication/authentication-service',
    'services/balance/balance-service',
    'services/translate/common',
    'components/header/header.component',
    'components/application/application.component',
    'components/landing/landing.component',
    'components/scoring/scoring.component',
    'components/estimate/estimate.component',
    'components/report/report.component',
    'components/report/questions/questions.component',
    'components/footer/footer.component',
    'components/common/project-card/project-card.component',
    'components/common/transaction-awaiting-modal/transaction-awaiting-modal.component',
    'components/common/receive-svt-modal/receive-svt-modal.component',
    'components/common/receive-ether-modal/receive-ether-modal.component',
    'components/common/project-information/project-information.component',
    'components/account/account.component'
  ]);
}
