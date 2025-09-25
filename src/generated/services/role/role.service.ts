import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { HttpClient, HttpParams } from "@angular/common/http";
import { ApiResponseModel, PagedResponseModel } from "../../../models/common/api-response.model";
import { RoleDetailsModel } from "../../../models/roles/role.model";
import { AllRolesPagingParams } from "../../../models/common/paging-params.model";

@Injectable({
  providedIn: 'root'
})
export class RoleService {
  private readonly _apiBaseUrl = environment.apiBaseUrl;

  constructor(private http: HttpClient) {}

  allRoles(pagingParams: AllRolesPagingParams) {
    let params = new HttpParams();

    if (pagingParams.sortOrder) params = params.append('sortOrder', pagingParams.sortOrder);
    if (pagingParams.search) params = params.append('search', pagingParams.search);
    if (pagingParams.status) params = params.append('status', pagingParams.status);
    if (pagingParams.type) params = params.append('type', pagingParams.type);
    if (pagingParams.level) params = params.append('level', pagingParams.level);
    if (pagingParams.page) params = params.append('page', pagingParams.page);
    if (pagingParams.size) params = params.append('size', pagingParams.size);
    
    return this.http.get<ApiResponseModel<PagedResponseModel<RoleDetailsModel>>>(`${this._apiBaseUrl}/api/jobs`, { params });
  }

  addRole(details: {}) {
    return this.http.post<ApiResponseModel<any>>(`${this._apiBaseUrl}/api/jobs`, details);
  }
}
