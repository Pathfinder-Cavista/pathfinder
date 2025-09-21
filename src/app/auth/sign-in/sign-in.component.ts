import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CarouselModule, OwlOptions } from 'ngx-owl-carousel-o';
import { AuthService } from '../../../generated/services/auth/auth.service';
import { Router } from '@angular/router';
import { AlertService } from '../../../generated/services/alert/alert.service';

@Component({
  selector: 'app-sign-in',
  imports: [
    CarouselModule,
    ReactiveFormsModule,
  ],
  templateUrl: './sign-in.component.html',
  styleUrl: './sign-in.component.scss'
})
export class SignInComponent {
  protected customOptions: OwlOptions = {
    loop: true,
    mouseDrag: true,
    touchDrag: true,
    pullDrag: true,
    dots: true,
    navSpeed: 700,
    autoplay: true,
    navText: ['', ''],
    items: 1,
    nav: false,
  };

  protected loginForm = new FormGroup({
    emailAddress: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required]),
  });

  constructor(private authService: AuthService, private router: Router, private alertService: AlertService) {}

  protected signIn() {
    this.alertService.showInfo('Please Wait', 'Signing you in...');
    this.authService.login(this.loginForm.value).subscribe({
      next: () => {
        this.alertService.showSuccess('Success', 'Sign in operation successful!')
        this.authService.fetchCurrentUser().subscribe({
          next: () => this.router.navigate(['/main/dashboard']),
          error: (err) => this.alertService.showDanger('Error', err.error.message)
        });
      },
      error: (err) => this.alertService.showDanger('Error', err.error.message)
    });
  }
}
