import {MultiTranslateLoader} from './multi-translate-loader';

export function multiTranslateLoaderFactory() {
  return new MultiTranslateLoader([
    'services/questions/questions',
    'components/header/header.component',
    'components/application/application.component'
  ]);
}
