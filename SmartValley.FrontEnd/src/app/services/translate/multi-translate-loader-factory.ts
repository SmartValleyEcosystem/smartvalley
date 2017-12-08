import {MultiTranslateLoader} from './multi-translate-loader';

export function multiTranslateLoaderFactory() {
  return new MultiTranslateLoader([
    {prefix: 'services/questions/questions-', suffix: '.json'},
    {prefix: 'components/header/header.component-', suffix: '.json'},
    {prefix: 'components/application/application.component-', suffix: '.json'}
  ]);
}
