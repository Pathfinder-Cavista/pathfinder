import { Component, OnDestroy, OnInit } from '@angular/core';
import { NavigationEnd, Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AuthService } from '../../../generated/services/auth/auth.service';
import { filter, Subscription } from 'rxjs';
import { NgClass } from '@angular/common';

@Component({
  selector: 'app-layout',
  imports: [
    RouterOutlet,
    RouterLink,
    RouterLinkActive,
    NgClass
],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.scss'
})
export class LayoutComponent implements OnInit, OnDestroy {
  protected currentRoute: string = '';
  protected subscription: Subscription[] = [];

  constructor(protected authService: AuthService, private router: Router) {}

  ngOnInit(): void {
    this.currentRoute = this.router.url;

    const sub = this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe((event: NavigationEnd) => {
      this.currentRoute = event.urlAfterRedirects;
    });
  }

  ngOnDestroy(): void {
    this.subscription.forEach(s => s.unsubscribe());
  }
}
