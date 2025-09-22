import { Routes } from "@angular/router";
import { LayoutComponent } from "./layout/layout.component";

export const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      {
        path: 'dashboard',
        loadComponent: () => import('./dashboard/dashboard.component').then(c => c.DashboardComponent),
        title: 'Dashboard | Path Finder'
      },
      {
        path: 'roles',
        loadChildren: () => import('./roles/roles.routes').then(r => r.routes),
        title: 'Roles | Path Finder'
      },
      {
        path: '',
        pathMatch: 'full',
        redirectTo: 'dashboard'
      }
    ]
  }
];
