import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { PresentService } from '../../../Service/Present/present-service';
import { DonorModel } from '../../../Models/DonorModel';
import { DonorService } from '../../../Service/Donor/donor-service';
import { CategoryModel } from '../../../Models/CategoryModel';

@Component({
  selector: 'app-add-present',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './add-present.html',
  styleUrl: './add-present.scss',
})
export class AddPresent implements OnInit {

  private fb = inject(FormBuilder);
  private presentService = inject(PresentService);
  private donorService = inject(DonorService);
  private router = inject(Router);

  addPresentForm!: FormGroup;
  donorsList: DonorModel[] = [];
  categoryList: CategoryModel[] = [];
  base64Image: string = '';

  formFields = [
    { name: 'name', label: 'שם מתנה', type: 'text', errorMsg: 'שם מתנה הוא שדה חובה.' },
    { name: 'description', label: 'תיאור', type: 'text', errorMsg: 'תיאור הוא שדה חובה.' },
    { name: 'price', label: 'מחיר', type: 'number', errorMsg: 'מחיר חייב להיות חיובי.' },
    { name: 'pictureUrl', label: 'קישור לתמונה', type: 'text', errorMsg: 'קישור לתמונה הוא חובה.' },
  ];

  ngOnInit() {
    this.addPresentForm = this.fb.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
      donorId: ['', Validators.required],
      categoryId: ['', Validators.required],
      price: ['', [Validators.required, Validators.min(0)]],
      pictureUrl: ['', Validators.required],
      purchasesAmount: [0, [Validators.required, Validators.min(0)]],
    });

    this.presentService.getCategories().subscribe({
      next: (data: CategoryModel[]) => {
        this.categoryList = data;
        console.log(this.categoryList);

      },
      error: (err: any) => {
        console.error('שגיאה בטעינת קטגוריות:', err);
      }
    });

    this.donorService.getDonors().subscribe({
      next: (data: DonorModel[]) => {
        this.donorsList = data;
      },
      error: (err: any) => {
        console.error('שגיאה בטעינת תורמים:', err);
      }
    });
  }

  onSubmit() {
    if (this.addPresentForm.valid) {
      const formValues = this.addPresentForm.value;
      const presentData = {
        Name: formValues.name,
        Description: formValues.description,
        Price: Number(formValues.price),
        PictureUrl: formValues.pictureUrl,
        PurchasesAmount: Number(formValues.purchasesAmount),
        DonorId: Number(formValues.donorId),
        CategoryId: Number(formValues.categoryId)
      };

      console.log("נתונים שנשלחים לשרת:", presentData);

      this.presentService.addPresent(presentData as any).subscribe({
        next: (response: any) => {
          alert('המתנה נוספה בהצלחה!');
          this.router.navigate(['api/present/getAll']);
        },
        error: (err: any) => {
          console.error("שגיאת שרת:", err);
          alert('חלה שגיאה בשמירה. בדקי את ה-Network Tab בדפדפן.');
        }
      });
    } else {
      this.addPresentForm.markAllAsTouched();
    }
  }

  onFileSelected(event: any) {
    const file: File = event.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = () => {
        this.base64Image = reader.result as string;
        this.addPresentForm.patchValue({
          pictureUrl: this.base64Image
        });
      };
      reader.readAsDataURL(file);
    }
  }
}