import { Component, OnDestroy, OnInit, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AlertComponent } from "./common/alert/alert.component";
import { AuthService } from '../generated/services/auth/auth.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    AlertComponent
  ],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App implements OnInit, OnDestroy {
  protected readonly title = signal('pathfinder-web');
  protected readonly isLoading = signal(true);
  protected readonly subscription: Subscription[] = [];

  constructor(private authService: AuthService) { }

  private getCurrentUser() {
    const sub = this.authService.fetchCurrentUser().subscribe({
      next: () => {
        this.isLoading.set(false);
      },
      error: () => {
        this.authService.logout();
        this.isLoading.set(false);
      },
    });

    this.subscription.push(sub);
  }

  ngOnInit(): void {
    this.getCurrentUser();
  }

  ngOnDestroy(): void {
    this.subscription.forEach(s => s.unsubscribe);
  }
}
