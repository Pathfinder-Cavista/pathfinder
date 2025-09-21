import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Alert } from '../../../models/common/alert.model';

@Injectable({
  providedIn: 'root',
})
export class AlertService {
  private alerts: Alert[] = [];
  private alertSubject = new BehaviorSubject<Alert[]>([]);

  alerts$ = this.alertSubject.asObservable();

  showDanger(title: string, message: string) {
    this.showAlert(title, message, 'danger');
  }
  
  showInfo(title: string, message: string) {
    this.showAlert(title, message, 'info');
  }
  
  showSuccess(title: string, message: string) {
    this.showAlert(title, message, 'success');
  }
  
  showWarning(title: string, message: string) {
    this.showAlert(title, message, 'warning');
  }

  showAlert(title: string, message: string, type: 'primary' | 'secondary' | 'success' | 'danger' | 'warning' | 'info' | 'light') {
    const id = new Date().getTime();
    const alert: Alert = { id, type, title, message };

    this.alerts = [...this.alerts, alert];
    this.alertSubject.next(this.alerts);

    // Auto close after 5 seconds
    setTimeout(() => {
      this.removeAlert(id);
    }, 5000);
  }

  closeAlert(id: number) {
    this.removeAlert(id);
  }

  closeAllAlerts() {
    this.alertSubject.next([]);
  }

  private removeAlert(id: number) {
    this.alerts = this.alerts.filter(alert => alert.id !== id);
    this.alertSubject.next(this.alerts);
  }
}
