import { Component } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { OAuthService } from 'angular-oauth2-oidc';

@Component({
  selector: 'app-callback',
  templateUrl: './callback.component.html',
  styleUrls: ['./callback.component.scss']
})
export class CallbackComponent {
  constructor(private authService: AuthService, private oauthService: OAuthService) { }
  accessToken: any;

  ngOnInit() {
    //this.oauthService.tryLogin();
    this.authService.completeLogin();
    this.accessToken = this.oauthService.getAccessToken();
    console.log("Token", this.accessToken);
  }

  logout() {
    //this.oauthService.logOut();
    this.authService.logout();
  }
}
