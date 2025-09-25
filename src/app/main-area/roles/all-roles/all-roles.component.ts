import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { RoleService } from '../../../../generated/services/role/role.service';
import { AlertService } from '../../../../generated/services/alert/alert.service';
import { PagedResponseModel } from '../../../../models/common/api-response.model';
import { RoleDetailsModel } from '../../../../models/roles/role.model';
import { DatePipe, NgClass } from '@angular/common';
import { RoleStatusBadgeComponent } from "./role-status-badge/role-status-badge.component";
import { AllRolesPagingParams } from '../../../../models/common/paging-params.model';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-all-roles',
  imports: [
    DatePipe,
    RoleStatusBadgeComponent,
    NgClass,
    FormsModule,
],
  templateUrl: './all-roles.component.html',
  styleUrl: './all-roles.component.scss'
})
export class AllRolesComponent implements OnInit, OnDestroy {
  protected pagedRoles?: PagedResponseModel<RoleDetailsModel>;
  protected isLoading: boolean = true;
  protected pagingParams: AllRolesPagingParams = new AllRolesPagingParams();
  protected searchPhrase: string = '';

  private subscription: Subscription[] = [];

  constructor(private roleService: RoleService, private alertService: AlertService, private cdr: ChangeDetectorRef) {}

  get pageRange() {
    return Array.from({ length: this.pagedRoles?.pageCount ?? 0 }, (_, i) => i);
  }

  protected searchRole() {
    this.pagingParams.search = this.searchPhrase;
    this.updatePage(1);
  }

  protected updatePage(page: number) {
    this.pagingParams.page = page;
    this.cdr.detectChanges();
    this.getAllRoles();
  }

  private getAllRoles() {
    this.isLoading = true;
    this.cdr.detectChanges();
    const sub = this.roleService.allRoles(this.pagingParams).subscribe({
      next: response => {
        this.pagedRoles = response.data;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: err => {
        this.alertService.showDanger('Error', err.error.message);
        this.pagedRoles = undefined;
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });

    this.subscription.push(sub);
  }

  ngOnInit(): void {
    this.getAllRoles();
  }

  ngOnDestroy(): void {
    this.subscription.forEach(s => s.unsubscribe());
  }
}
