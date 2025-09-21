import { Routes } from '@angular/router';
import { authGuard } from '../generated/guards/auth/auth.guard';

export const routes: Routes = [
    {
        path: 'main',
        canActivate: [authGuard],
        loadChildren: () => import('./main-area/main-area.routes').then(r => r.routes)
    },
    {
        path: '',
        pathMatch: 'full',
        loadChildren: () => import('./auth/auth.routes').then(r => r.routes)
    },
];
