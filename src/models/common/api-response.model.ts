export interface ApiResponseModel<T> {
  status: number,
  message: string,
  success: boolean,
  data: T,
}

export interface PagedResponseModel<T> {
  currentPage: number,
  pageCount: number,
  itemCount: number,
  hasPrevious: boolean,
  hasNext: boolean,
  items: T[],
}