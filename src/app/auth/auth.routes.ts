import { Routes } from '@angular/router';

export const routes: Routes = [
    {
        path: '',
        title: 'Sign In | Path Finder',
        loadComponent: () => import('./sign-in/sign-in').then(c => c.SignIn),
    }
];