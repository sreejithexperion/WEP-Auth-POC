import { Component } from '@angular/core';
import { CommunicationService } from 'src/app/services/communication.service';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.scss']
})
export class LoginPageComponent {
  constructor(private communicationService: CommunicationService, private authService: AuthService, private router: Router, private http: HttpClient) { }

  ngOnInit() {
    // Listen for messages from the popup
    window.addEventListener('message', (event) => {
      // Verify the origin of the message
      if (event.origin === 'http://localhost:4201') {
        this.communicationService.sendValue(event.data);
        var accessToken = event.data;
        localStorage.setItem("accesstoken", accessToken);
        this.authService.setToken(accessToken);
        this.router.navigate(['/dashboard']);
        //this.authenticateApp(accessToken);
      }
    });
  }

  openNewWindow(): void {
    var authToken = localStorage.getItem('accesstoken');
    if(authToken) {
      this.router.navigate(['/dashboard']);
    } else {
      var app1Token = localStorage.getItem('access_token');
      if(app1Token) {
        const grant_type = "urn:ietf:params:oauth:grant-type:token-exchange";
        const subject_token_type = "urn:ietf:params:oauth:token-type:access_token";
        const audience = 'wep_client';
        const client_id = 'wep_client';

        var authority = 'https://localhost:5162';

        const body = new HttpParams()
            .set('grant_type', grant_type)
            .set('client_id', client_id)
            .set('subject_token', app1Token)
            .set('subject_token_type', subject_token_type)
            .set('audience', audience)
            .set('scope', 'wep_client');

        this.http.post(`${authority}/connect/token`, body, {
          headers: new HttpHeaders({
            'Content-Type': 'application/x-www-form-urlencoded'
          })
        }).subscribe((acctokens: any) => {
          localStorage.setItem('accesstoken', acctokens.access_token);
          localStorage.setItem('ex_id_token', acctokens.access_token);
          this.router.navigate(['/dashboard']);
        });
      } else {
        const width = 900;
        const height = 700;
        const left = (screen.width / 2) - (width / 2);
        const top = (screen.height / 2) - (height / 2);

        window.open("http://localhost:4201/login", 'popup', `width=${width},height=${height},top=${top},left=${left},resizable=yes,scrollbars=yes,toolbar=no,menubar=no,location=no,directories=no,status=yes`);
      }
    }
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
