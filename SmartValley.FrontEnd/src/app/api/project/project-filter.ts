import {ProjectsOrderBy} from '../application/projects-order-by.enum';
import {SortDirection} from '../sort-direction.enum';

export interface ProjectFilter {
  offset: number;
  count: number;
  searchString?: string;
  stageType?: number;
  countryCode?: string;
  categoryType?: number;
  minimumScore?: number;
  maximumScore?: number;
  orderBy?: ProjectsOrderBy;
  direction?: SortDirection;
}
