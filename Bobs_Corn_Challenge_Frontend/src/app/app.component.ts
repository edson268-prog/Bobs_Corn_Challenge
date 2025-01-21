import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  constructor(private router: Router) { }

  title = 'Bobs_Corn_Challenge_Frontend';

  navigateTo(route: string) {
    console.log(`Navigating to ${route}`);
    this.router.navigate([route]);
  }
}
