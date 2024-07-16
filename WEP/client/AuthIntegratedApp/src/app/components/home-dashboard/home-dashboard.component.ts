import { Component } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';
import { HttpParams } from '@angular/common/http';

@Component({
  selector: 'app-home-dashboard',
  templateUrl: './home-dashboard.component.html',
  styleUrls: ['./home-dashboard.component.scss']
})

export class HomeDashboardComponent {
  value1: string = "";
  value2: string = "";
  //private clientId = 'pkce_client';
  private authority = 'https://localhost:5162'; // URL of your IdentityServer
  //private redirectUri = 'http://localhost:4201/callback';
  private postLogoutRedirectUri = 'http://localhost:4200';
  //private responseType = 'code';
  //private scope = 'openid profile testIdentityServer4mvc';

  constructor(private authService: AuthService, private router: Router) { };

  ngOnInit() {
    this.loadDashboardData();
  }

  loadDashboardData() {
    this.authService.getDashboardData().subscribe(
      response => {
        if(response && response.length > 0) {
          this.value1 = response[0];
        }
      }
    )
  }

  logout() {
    const params = new HttpParams()
      .set('post_logout_redirect_uri', this.postLogoutRedirectUri);
    const logoutUrl = `${this.authority}/connect/endsession?ipost_logout_redirect_uri=${this.postLogoutRedirectUri}`;
    window.location.href = logoutUrl;
    // localStorage.removeItem('access_token');
    // localStorage.removeItem('id_token');
    // localStorage.removeItem('accesstoken');
    localStorage.removeItem('accesstoken');
    localStorage.removeItem('access_token');
    this.router.navigate(['/login']);
  }
}
