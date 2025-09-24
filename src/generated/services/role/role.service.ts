import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { ApiResponseModel, PagedResponseModel } from "../../../models/common/api-response.model";
import { RoleDetailsModel } from "../../../models/roles/role.model";

@Injectable({
  providedIn: 'root'
})
export class RoleService {
  private readonly _apiBaseUrl = environment.apiBaseUrl;

  constructor(private http: HttpClient) {}

  allRoles() {
    return this.http.get<ApiResponseModel<PagedResponseModel<RoleDetailsModel>>>(`${this._apiBaseUrl}/api/jobs`);
  }

  addRole(details: {}) {
    return this.http.post<ApiResponseModel<any>>(`${this._apiBaseUrl}/api/jobs`, details);
  }
}
