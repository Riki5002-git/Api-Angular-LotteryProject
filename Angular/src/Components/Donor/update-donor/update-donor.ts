import { Component } from '@angular/core';
import { OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DonorService } from '../../../Service/Donor/donor-service';
import { Router, ActivatedRoute } from '@angular/router';
import { DonorModel } from '../../../Models/DonorModel';
import { take } from 'rxjs';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-update-donor',
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './update-donor.html',
  styleUrl: './update-donor.scss',
})
export class UpdateDonor implements OnInit {

  constructor(private fb: FormBuilder, private donorService: DonorService, private router: Router, private route: ActivatedRoute) { }

  updateDonorForm!: FormGroup;
  donorsList: DonorModel[] = [];
  donorId!: number;

  formFields = [
    { name: 'firstName', label: 'שם פרטי', type: 'text', errorMsg: 'שדה זה הוא שדה חובה.' },
    { name: 'lastName', label: 'שם משפחה', type: 'text', errorMsg: 'שדה זה הוא שדה חובה.' },
    { name: 'userName', label: 'שם משתמש', type: 'text', errorMsg: 'שדה זה הוא שדה חובה.' },
    { name: 'password', label: 'סיסמה', type: 'password', errorMsg: 'שדה זה הוא שדה חובה.' },
    { name: 'email', label: 'אימייל', type: 'email', errorMsg: 'אימייל לא תקין או חסר.' },
    { name: 'phone', label: 'טלפון', type: 'text', errorMsg: 'שדה זה הוא שדה חובה.' },
  ];

  ngOnInit() {
    this.updateDonorForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      userName: ['', Validators.required],
      password: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', Validators.required],
    });

    this.donorId = Number(this.route.snapshot.paramMap.get('id'));
    this.loadInitialData();
    if (this.donorId) {
      this.loadDonorDetails();
    }
  }

  loadInitialData() {
    this.donorService.getDonors().subscribe(data => this.donorsList = data);
  }

  loadDonorDetails() {
    this.donorService.getDonors().pipe(take(1)).subscribe(donors => {
      const donor = donors.find(p => p.id === this.donorId);
      if (donor) {
        this.updateDonorForm.patchValue({
          firstName: donor.firstName,
          lastName: donor.lastName,
          userName: donor.userName,
          password: donor.password,
          email: donor.email,
          phone: donor.phone
        });
      }
    });
  }

  onSubmit() {
    if (this.updateDonorForm.valid) {
      const formValues = this.updateDonorForm.value;
      const updatedData = {
        id: this.donorId,
        firstName: formValues.firstName,
        lastName: formValues.lastName,
        userName: formValues.userName,
        password: formValues.password,
        email: formValues.email,
        phone: formValues.phone
      };

      this.donorService.updateDonor(updatedData as any).subscribe({
        next: () => {
          alert('התורם עודכן בהצלחה!');
          this.router.navigate(['api/Donor/getAll']);
        },
        error: (err) => {
          console.error("שגיאת שרת:", err);
          alert('חלה שגיאה בעדכון.');
        }
      });
    }
  }

  onFileSelected(event: any) {
    const file: File = event.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = () => {
        this.updateDonorForm.patchValue({ pictureUrl: reader.result as string });
      };
      reader.readAsDataURL(file);
    }
  }
}
