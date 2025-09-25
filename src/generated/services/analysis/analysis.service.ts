import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { ApplicationsByLocationResponse, JobDistributionResponseModel } from "../../../models/analysis/analysis.model";

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
    return this.http.get<ApplicationsByLocationResponse[]>(`${this._apiBaseUrl}/api/analytics/applications-by-location`, { headers });
  }
}