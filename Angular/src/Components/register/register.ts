import { Component, OnInit, inject } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { PersonModel } from '../../Models/PersonModel';
import { PersonService } from '../../Service/Person/PersonService';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './register.html',
  styleUrl: './register.scss',
})
export class Register implements OnInit {
  private fb = inject(FormBuilder);
  registerForm!: FormGroup;
  usersList: PersonModel[] = [];
  formFields = [
    { name: 'firstName', label: 'שם פרטי', type: 'text', errorMsg: 'שדה זה הוא חובה.' },
    { name: 'lastName', label: 'שם משפחה', type: 'text', errorMsg: 'שדה זה הוא חובה.' },
    { name: 'userName', label: 'שם משתמש', type: 'text', errorMsg: 'שדה זה הוא חובה.' },
    { name: 'password', label: 'סיסמה', type: 'password', errorMsg: 'סיסמה היא חובה (מינימום 3 תווים).' },
    { name: 'email', label: 'אימייל', type: 'email', errorMsg: 'כתובת אימייל לא תקינה.' },
    { name: 'phone', label: 'טלפון', type: 'tel', errorMsg: 'נא להזין מספרים בלבד.' }
  ];

  constructor(private personService: PersonService, private router: Router) { }

  ngOnInit() {
    this.registerForm = this.fb.group({
      firstName: ['', [Validators.required]],
      lastName: ['', [Validators.required]],
      userName: ['', [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', [Validators.required, Validators.pattern('^[0-9]+$')]]
    });

    this.personService.getPeople().subscribe(data => {
      this.usersList = data;
    });
  }

  checkDuplicate(): boolean {
    if (!this.usersList) return false;
    const currentUserName = this.registerForm.get('userName')?.value;
    return this.usersList.some(u => u.userName === currentUserName);
  }

  onSubmit() {
    if (this.registerForm.valid) {
      if (this.checkDuplicate()) {
        alert("משתמש קיים במערכת. שם המשתמש תפוס.");
        return;
      }

      const userData: PersonModel = this.registerForm.value;

      this.personService.register(userData).subscribe({
        next: (response) => {
          alert('נרשמת בהצלחה!');
          this.router.navigate(['/']);
        },
        error: (err) => {
          if (err.status === 400 || err.status === 500) {
            alert('הרישום נכשל: שגיאה בנתונים או בשרת');
          } else {
            alert('קרתה שגיאה לא צפויה - בדקי שהשרת רץ');
          }
        }
      });
    }
  }
}