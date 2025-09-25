export class AllRolesPagingParams {
  sortOrder: 'asc' | 'desc' = 'desc';
  search?: string;
  status?: 0 | 1 | 2 | 3;
  type?: 0 | 1 | 2 | 3 | 4;
  level?: 0 | 1 | 2 | 3 | 4 | 5 | 6;
  page: number = 1;
  size: number = 10;
}