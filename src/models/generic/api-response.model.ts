export interface ApiResponseModel<T> {
    status: number,
    message: string,
    success: boolean,
    data: T,
}