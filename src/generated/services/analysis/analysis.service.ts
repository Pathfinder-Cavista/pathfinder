import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { ApplicationsByLocationResponseModel, ApplicationsByStatusResponseModel, ApplicationsPerJobResponseModel, ApplicationTrendForYearResponseModel, ApplicationTrendResponseModel, HireRateByJobtypeResponseModel, JobDistributionResponseModel, OpenRolesDurationResponseModel } from "../../../models/analysis/analysis.model";

@Injectable({
  providedIn: 'root'
})
export class AnalysisService {
  private readonly _apiBaseUrl = environment.apiBaseUrl;
  private insightHeaders: HttpHeaders = new HttpHeaders({ 'X-Insights': 'd54098cbQ8eb5R4a53S8498T5004611355bd' })

  constructor(private http: HttpClient) {}

  getAverageTimeToFill() {
    const headers = this.insightHeaders;
    return this.http.get<number>(`${this._apiBaseUrl}/api/analytics/average-time-to-fill`, { headers });
  }

  getJobStatusDistribution() {
    const headers = this.insightHeaders;
    return this.http.get<JobDistributionResponseModel[]>(`${this._apiBaseUrl}/api/analytics/jobstatus-distribution`, { headers });
  }

  getApplicationsByLocation() {
    const headers = this.insightHeaders;
    return this.http.get<ApplicationsByLocationResponseModel[]>(`${this._apiBaseUrl}/api/analytics/applications-by-location`, { headers });
  }

  getApplicationsPerJob() {
    const headers = this.insightHeaders;
    return this.http.get<ApplicationsPerJobResponseModel[]>(`${this._apiBaseUrl}/api/analytics/applications-per-job`, { headers });
  }
  
  getApplicationsTrend() {
    const headers = this.insightHeaders;
    return this.http.get<ApplicationTrendResponseModel[]>(`${this._apiBaseUrl}/api/analytics/applications-trend`, { headers });
  }
  
  getApplicationsTrendForYear(year: number) {
    const headers = this.insightHeaders;
    return this.http.get<ApplicationTrendForYearResponseModel[]>(`${this._apiBaseUrl}/api/analytics/applications-trend/${year}`, { headers });
  }
  
  getHireRateByJobtype() {
    const headers = this.insightHeaders;
    return this.http.get<HireRateByJobtypeResponseModel[]>(`${this._apiBaseUrl}/api/analytics/hire-rate-by-jobtype`, { headers });
  }
  
  getApplicationsByStatus() {
    const headers = this.insightHeaders;
    return this.http.get<ApplicationsByStatusResponseModel[]>(`${this._apiBaseUrl}/api/analytics/application-by-status`, { headers });
  }
  
  getOpenRolesDuration() {
    const headers = this.insightHeaders;
    return this.http.get<OpenRolesDurationResponseModel[]>(`${this._apiBaseUrl}/api/analytics/open-roles-durations`, { headers });
  }
  
}