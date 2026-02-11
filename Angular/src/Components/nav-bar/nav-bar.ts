import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { PersonService } from '../../Service/Person/PersonService';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterModule, CommonModule],
  templateUrl: './nav-bar.html',
  styleUrl: './nav-bar.scss'
})
export class NavBar {

  constructor(public personService: PersonService, private router: Router) { }

  onLogout() {
    this.personService.logout();
    this.router.navigate(['/api/person/login']);
  }
}
