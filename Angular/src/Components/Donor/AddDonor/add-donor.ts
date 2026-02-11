import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { DonorService } from '../../../Service/Donor/donor-service';
import { DonorModel } from '../../../Models/DonorModel';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-add-donor',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './add-donor.html',
  styleUrl: './add-donor.scss'
})
export class AddDonor implements OnInit {

  addDonorForm!: FormGroup;
  donorsList: DonorModel[] = [];

  formFields = [
    { name: 'firstName', label: 'שם פרטי', type: 'text', errorMsg: 'שדה זה הוא שדה חובה.' },
    { name: 'lastName', label: 'שם משפחה', type: 'text', errorMsg: 'שדה זה הוא שדה חובה.' },
    { name: 'userName', label: 'שם משתמש', type: 'text', errorMsg: 'שדה זה הוא שדה חובה.' },
    { name: 'password', label: 'סיסמה', type: 'password', errorMsg: 'שדה זה הוא שדה חובה.' },
    { name: 'email', label: 'אימייל', type: 'email', errorMsg: 'אימייל לא תקין או חסר.' },
    { name: 'phone', label: 'טלפון', type: 'text', errorMsg: 'שדה זה הוא שדה חובה.' },
  ];

  constructor(
    private donorService: DonorService,
    private router: Router,
    private fb: FormBuilder
  ) { }

  ngOnInit() {
    this.addDonorForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      userName: ['', Validators.required],
      password: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', Validators.required],
    });

    this.donorService.getDonors().subscribe(data => {
      this.donorsList = data;
    });
  }

  checkDuplicate(): boolean {
    if (!this.donorsList) return false;
    const currentUserName = this.addDonorForm.get('userName')?.value;
    return this.donorsList.some(u => u.userName === currentUserName);
  }

  onSubmit() {
    if (this.addDonorForm.valid) {
      if (this.checkDuplicate()) {
        alert("משתמש קיים במערכת. שם המשתמש תפוס.");
        return;
      }
      const newDonor: DonorModel = this.addDonorForm.value;
      this.saveAndNavigate(newDonor);
    }
  }

  saveAndNavigate(donor: DonorModel): void {
    this.donorService.addDonor(donor).subscribe({
      next: (data) => {
        console.log('תורם נוסף בהצלחה:', data);
        this.router.navigate(['/api/Donor/getAll']);
      },
      error: (err) => {
        console.error('שגיאה בשמירת התורם:', err);
        alert('תורם קיים כבר במערכת.');
      }
    });
  }
}