import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { Subscription } from 'rxjs';
import { trigger, transition, style, animate } from '@angular/animations';
import { AlertService } from '../../../generated/services/alert/alert.service';
import { Alert } from '../../../models/common/alert.model';
import { NgClass } from '@angular/common';

@Component({
  selector: 'app-alerts',
  imports: [
    NgClass
  ],
  templateUrl: './alert.component.html',
  styleUrls: ['./alert.component.scss'],
  animations: [
    trigger('alertState', [
      transition(':enter', [
        style({ transform: 'translateX(100%)', opacity: 0 }),
        animate('0.5s ease-out', style({ transform: 'translateX(0)', opacity: 1 })),
      ]),
      transition(':leave', [
        animate('0.5s ease-in', style({ transform: 'translateX(100%)', opacity: 0 })),
      ]),
    ]),
  ],
})
export class AlertComponent implements OnInit, OnDestroy {
  alerts: Alert[] = [];
  private subscription: Subscription = new Subscription();

  constructor(private alertService: AlertService, private cdr: ChangeDetectorRef) {}

  closeAlert(id: number) {
    this.alertService.closeAlert(id);
  }

  getIconForAlert(type: string) {
    const icons: { [key: string]: string } = {
      primary: 'ri-alert-line',
      secondary: 'ri-information-line',
      success: 'ri-check-line',
      danger: 'ri-error-warning-line',
      warning: 'ri-alert-line',
      info: 'ri-information-line',
      light: 'ri-sun-line',
    };
    return icons[type] || 'ri-alert-line';
  }

  
  ngOnInit() {
    this.subscription = this.alertService.alerts$.subscribe({
      next: alerts => {
        this.alerts = alerts;
        this.cdr.detectChanges();
      }
    });
  }

  ngOnDestroy() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
