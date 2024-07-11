import { Component } from '@angular/core';
import { CommunicationService } from 'src/app/services/communication.service';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.scss']
})
export class LoginPageComponent {
  constructor(private communicationService: CommunicationService, private authService: AuthService, private router: Router) { }

  ngOnInit() {
    // Listen for messages from the popup
    window.addEventListener('message', (event) => {
      // Verify the origin of the message
      if (event.origin === 'http://localhost:4201') {
        this.communicationService.sendValue(event.data);
        var accessToken = event.data;
        this.authService.setToken(accessToken);
        this.router.navigate(['/dashboard']);
        //this.authenticateApp(accessToken);
      }
    });
  }

  openNewWindow(): void {
    const width = 600;
    const height = 400;
    const left = (screen.width / 2) - (width / 2);
    const top = (screen.height / 2) - (height / 2);

    window.open("http://localhost:4201/login", 'popup', `width=${width},height=${height},top=${top},left=${left},resizable=yes,scrollbars=yes,toolbar=no,menubar=no,location=no,directories=no,status=yes`);
  }

  authenticateApp(accessToken: string) {
    this.authService.login({accessToken: accessToken}).subscribe(
      response => {
        console.log('Login successful', response);
        if(response && response.token) {
          this.authService.setToken(response.token);
          this.router.navigate(['/dashboard']);
        }
        // var tokenValue = "";
        // if(response && response.token)
        // {
        //   tokenValue = response.token;
        // }
        // window.opener.postMessage(tokenValue, 'http://localhost:4201');
        // window.close();
        // Handle successful login, e.g., redirect to another page
      },
      error => {
        console.error('Login failed', error);
        // Handle login failure, e.g., show an error message
      }
    );
  }
}
