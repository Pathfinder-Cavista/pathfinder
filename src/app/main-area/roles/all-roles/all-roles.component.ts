import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { RoleService } from '../../../../generated/services/role/role.service';
import { AlertService } from '../../../../generated/services/alert/alert.service';
import { PagedResponseModel } from '../../../../models/common/api-response.model';
import { RoleDetailsModel } from '../../../../models/roles/role.model';
import { DatePipe } from '@angular/common';
import { RoleStatusBadgeComponent } from "./role-status-badge/role-status-badge.component";

@Component({
  selector: 'app-all-roles',
  imports: [
    DatePipe,
    RoleStatusBadgeComponent,
],
  templateUrl: './all-roles.component.html',
  styleUrl: './all-roles.component.scss'
})
export class AllRolesComponent implements OnInit, OnDestroy {
  protected pagedRoles?: PagedResponseModel<RoleDetailsModel>;
  protected isLoading: boolean = true;

  private subscription: Subscription[] = [];

  constructor(private roleService: RoleService, private alertService: AlertService) {}

  private getAllRoles() {
    const sub = this.roleService.allRoles().subscribe({
      next: response => {
        this.pagedRoles = response.data;
        this.isLoading = false;
      },
      error: err => {
        this.alertService.showDanger('Error', err.error.message);
        this.isLoading = false;
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
