import { Component } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-home-dashboard',
  templateUrl: './home-dashboard.component.html',
  styleUrls: ['./home-dashboard.component.scss']
})

export class HomeDashboardComponent {
  value1: string = "";
  value2: string = "";

  constructor(private authService: AuthService) { };

  ngOnInit() {
    this.loadDashboardData();
  }

  loadDashboardData() {
    this.authService.getDashboardData().subscribe(
      response => {
        if(response && response.length > 0) {
          this.value1 = response[0];
          this.value2 = response[1];
        }
      }
    )
  }
}
