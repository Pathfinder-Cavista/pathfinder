import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CarouselModule, OwlOptions } from 'ngx-owl-carousel-o';
import { AuthService } from '../../../generated/services/auth/auth.service';
import { Router } from '@angular/router';

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
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required]),
  });

  constructor(private authService: AuthService, private router: Router) {}

  protected signIn() {
    this.authService.login(this.loginForm.value).subscribe({
      next: () => {
        this.authService.fetchCurrentUser().subscribe({
          next: () => this.router.navigate(['/dashboard']),
          error: () => console.warn('Failed to load user info')
        });
      },
      error: () => console.error('Login failed')
    });
  }
}
