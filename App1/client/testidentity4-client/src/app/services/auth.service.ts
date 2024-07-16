import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';

// const hashedValue = CryptoJS.SHA('secret').toString();
// console.log(hashedValue);

@Injectable({
  providedIn: 'root'
})

export class AuthService {
  private clientId = 'pkce_client';
  private authority = 'https://localhost:5212'; // URL of your IdentityServer
  private redirectUri = 'http://localhost:4201/callback';
  private postLogoutRedirectUri = 'http://localhost:4201';
  private responseType = 'code';
  private scope = 'openid profile testIdentityServer4mvc';

  constructor(private http: HttpClient, private router: Router) { }

  login() {
    const params = new HttpParams()
      .set('client_id', this.clientId)
      .set('redirect_uri', this.redirectUri)
      .set('response_type', this.responseType)
      .set('scope', this.scope);

    window.location.href = `${this.authority}/connect/authorize?${params.toString()}`;
  }

  completeLogin() {
    const urlParams = new URLSearchParams(window.location.search);
    const code = urlParams.get('code');
    const pkce_verifier = sessionStorage.getItem("PKCE_verifier");

    //var pkce_verifier = localStorage.getItem("PKCE_verifier") ? localStorage.getItem("PKCE_verifier"): '';

    if (code && pkce_verifier) {
      const body = new HttpParams()
        .set('grant_type', 'authorization_code')
        .set('client_id', this.clientId)
        .set('redirect_uri', this.redirectUri)
        .set('code', code)
        .set('code_verifier', pkce_verifier)
        .set('scope', 'testIdentityServer4mvc');

      this.http.post(`${this.authority}/connect/token`, body, {
        headers: new HttpHeaders({
          'Content-Type': 'application/x-www-form-urlencoded'
        })
      }).subscribe((tokens: any) => {
        localStorage.setItem('access_token', tokens.access_token);
        localStorage.setItem('id_token', tokens.id_token);
        this.exchangeToken(tokens.access_token);
        // if(this.getSource() === 'wep' && this.getAppKey() === 'xvbhjjj') {
        //   this.exchangeToken(tokens.access_token);
        // }
      });
    }
  }

  logout() {
    const params = new HttpParams()
      .set('post_logout_redirect_uri', this.postLogoutRedirectUri);
    const idToken = localStorage.getItem('id_token');
    const logoutUrl = `${this.authority}/connect/endsession?id_token_hint=${idToken}&post_logout_redirect_uri=${this.postLogoutRedirectUri}`;
    window.location.href = logoutUrl;
    localStorage.removeItem('access_token');
    localStorage.removeItem('id_token');
    localStorage.removeItem('accesstoken');
    //window.location.href = `${this.authority}/connect/endsession?${params.toString()}`;
  }

  exchangeToken(accessToken: any) {
    const grant_type = "urn:ietf:params:oauth:grant-type:token-exchange";
    const subject_token_type = "urn:ietf:params:oauth:token-type:access_token";
    const audience = 'wep_client';
    const client_id = 'wep_client';

    var authority = 'https://localhost:5162';

    const body = new HttpParams()
        .set('grant_type', grant_type)
        .set('client_id', client_id)
        .set('subject_token', accessToken)
        .set('subject_token_type', subject_token_type)
        .set('audience', audience)
        .set('scope', 'wep_client');
        //.set('resource', 'https://localhost:8900');

      this.http.post(`${authority}/connect/token`, body, {
        headers: new HttpHeaders({
          'Content-Type': 'application/x-www-form-urlencoded'
        })
      }).subscribe((acctokens: any) => {
        localStorage.setItem('ex_access_token', acctokens.access_token);
        localStorage.setItem('ex_id_token', acctokens.access_token);
        window.opener.postMessage(acctokens.access_token, 'http://localhost:4200');
        window.close();
      });
    }

  get isAuthenticated(): boolean {
    return !!localStorage.getItem('access_token');
  }

  setSource(source: string) {
    sessionStorage.setItem('source', source);
  }

  setAppKey(appKey: string) {
    sessionStorage.setItem('appKey', appKey);
  }

  getSource() {
    return sessionStorage.getItem('source');
  }

  getAppKey() {
    return sessionStorage.getItem('appKey');
  }
}
