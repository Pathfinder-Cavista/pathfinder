import { Routes } from "@angular/router";

export const routes: Routes = [
  {
    path: 'all',
    loadComponent: () => import('./all-roles/all-roles.component').then(c => c.AllRolesComponent),
    title: 'All Roles | Path Finder', 
  },
  {
    path: 'add',
    loadComponent: () => import('./add-role/add-role.component').then(c => c.AddRoleComponent),
    title: 'Add Role | Path Finder', 
  },
]