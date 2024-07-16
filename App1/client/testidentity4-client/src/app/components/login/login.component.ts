import { Component, OnDestroy, OnInit } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { AuthConfig, JwksValidationHandler } from 'angular-oauth2-oidc';
import { Subscription } from 'rxjs';
import { OAuthService, OAuthEvent } from 'angular-oauth2-oidc';
import { ActivatedRoute } from '@angular/router';
//import { JwksValidationHandler } from 'angular-oauth2-oidc-jwks';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit, OnDestroy {
  private clientId = 'pkce_client';
  private authority = 'https://localhost:5000'; // URL of your IdentityServer
  private redirectUri = 'http://localhost:4200/callback';
  private postLogoutRedirectUri = 'http://localhost:4200';
  private responseType = 'code';
  private scope = 'openid profile api1';
  claims: any;
  accessToken = '';
  //$oauthSubscription: Subscription;

  authConfig: AuthConfig = {
    issuer: 'https://localhost:5212',
    redirectUri: 'http://localhost:4201/callback',
    clientId: 'pkce_client',
    scope: 'openid profile testIdentityServer4mvc',
    responseType: 'code',
    postLogoutRedirectUri: 'http://localhost:4201',
    showDebugInformation: true,
    timeoutFactor: 0.75,
    sessionChecksEnabled: true,
    clearHashAfterLogin: false,
    nonceStateSeparator: ',',
    dummyClientSecret: 'test',
    //usePkce: true
  }

  constructor(
    private http: HttpClient, 
    private router: Router, 
    private authService: AuthService, 
    private oauthService: OAuthService, 
    private route: ActivatedRoute) {
      //this.configureWithNewConfigApi();
  }

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      const src = params['src'];
      const appKey = params['appkey'];
      this.authService.setSource(src);
      this.authService.setAppKey(appKey);
      console.log(src + appKey);
      this.configureWithNewConfigApi();
    });
  }

  private configureWithNewConfigApi() {
    this.oauthService.configure(this.authConfig);
    //this.oauthService.loadDiscoveryDocumentAndLogin();
    this.oauthService.tokenValidationHandler = new JwksValidationHandler();
    //this.oauthService.loadDiscoveryDocumentAndLogin();
    
    this.oauthService.loadDiscoveryDocumentAndLogin().then(_ => {
      if (!this.oauthService.hasValidIdToken() ||
        !this.oauthService.hasValidAccessToken()) {
        this.oauthService.initCodeFlow();
        //this.oauthService.initImplicitFlow();
      } else {
        this.router.navigate(['/']);
      }
    })
  }

  ngOnDestroy() {
    //this.$oauthSubscription.unsubscribe();
  }

  login() {
    this.oauthService.initCodeFlow();
  }

  logout() {
    this.authService.logout();
  }

  get isAuthenticated() {
    return this.authService.isAuthenticated;
  }
}
