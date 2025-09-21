import { Routes } from "@angular/router";
import { LayoutComponent } from "./layout/layout.component";
import { DashboardComponent } from "./dashboard/dashboard.component";

export const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      {
        path: 'dashboard',
        component: DashboardComponent,
        title: 'Dashboard | Path Finder'
      },
      {
        path: '',
        pathMatch: 'full',
        redirectTo: 'dashboard'
      }
    ]
  }
];
