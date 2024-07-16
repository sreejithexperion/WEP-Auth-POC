import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';

@Component({
  selector: 'app-modal-login',
  templateUrl: './modal-login.component.html',
  styleUrls: ['./modal-login.component.scss']
})

export class ModalLoginComponent {
  constructor(private router: Router, private http: HttpClient, public dialogRef: MatDialogRef<ModalLoginComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

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
    this.dialogRef.close();
  }

  openConnect() {
    alert("Connect login");
    this.dialogRef.close();
  }

  openBoost() {
    alert("Boost login");
    this.dialogRef.close();
  }
}
