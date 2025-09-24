import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-role-status-badge',
  imports: [],
  templateUrl: './role-status-badge.component.html',
  styleUrl: './role-status-badge.component.scss'
})
export class RoleStatusBadgeComponent {
  @Input() status: string = ''
}
