export interface JobDistributionResponseModel {
  status: number,
  statusText: string,
  count: number,
}

export interface ApplicationsByLocationResponse {
  location: string,
  applications: number,
}