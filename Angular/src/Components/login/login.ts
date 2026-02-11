import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { PersonService } from '../../Service/Person/PersonService';
import { Router } from '@angular/router';
import { signal } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
    selector: 'app-login',
    standalone: true,
    imports: [FormsModule, CommonModule, RouterLink],
    templateUrl: './login.html',
    styleUrl: './login.scss',
})
export class Login {

    username: string = '';
    password: string = '';
    errorMessage = signal<string>('');

    constructor(private personService: PersonService, private router: Router) { }

    login() {
        this.errorMessage.set('');
        this.personService.login(this.username, this.password).subscribe({
            next: (res) => this.router.navigate(['/api/present/getAll']),
            error: (err) => {
                this.errorMessage.set('שם משתמש או סיסמה שגויים.');
            }
        });
    }
}