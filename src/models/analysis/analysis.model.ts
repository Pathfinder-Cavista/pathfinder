export interface JobDistributionResponseModel {
  status: number,
  statusText: string,
  count: number,
}

export interface ApplicationsByLocationResponseModel {
  location: string,
  applications: number,
}

export interface ApplicationsPerJobResponseModel {
  jobId: string,
  jobTitle: string,
  count: number,
}

export interface ApplicationTrendResponseModel {
  date: string,
  count: number,
}

export interface ApplicationTrendForYearResponseModel {
  monthKey: number,
  month: string,
  applications: number,
}

export interface HireRateByJobtypeResponseModel {
  jobType: number,
  jobTypeText: string,
  hires: number,
}

export interface ApplicationsByStatusResponseModel {
  status: number,
  statusText: string,
  count: number,
}

export interface OpenRolesDurationResponseModel {
  roleId: string,
  roleTitle: string,
  status: number,
  statusText: string,
  openDays: number,
}