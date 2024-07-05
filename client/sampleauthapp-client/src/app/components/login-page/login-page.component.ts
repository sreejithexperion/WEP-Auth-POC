import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.scss']
})

export class LoginPageComponent {
  public loginForm!: FormGroup;
  constructor(private formbuilder: FormBuilder,private http: HttpClient, private router: Router, private authService: AuthService) { }

  ngOnInit(): void {
    this.loginForm = this.formbuilder.group({
      username: [''],
      password: ['', Validators.required]
    })
  }

  login() {
    this.authService.login(this.loginForm.value).subscribe(
      response => {
        console.log('Login successful', response);
        var tokenValue = "";
        if(response && response.token)
        {
          tokenValue = response.token;
        }
        window.opener.postMessage(tokenValue, 'http://localhost:4200');
        window.close();
        // Handle successful login, e.g., redirect to another page
      },
      error => {
        console.error('Login failed', error);
        // Handle login failure, e.g., show an error message
      }
    );
  } 
}
