import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { PresentService } from '../../../Service/Present/present-service';
import { DonorModel } from '../../../Models/DonorModel';
import { DonorService } from '../../../Service/Donor/donor-service';
import { CategoryModel } from '../../../Models/CategoryModel';
import { take } from 'rxjs';

@Component({
  selector: 'app-update-present',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './update-present.html',
  styleUrl: './update-present.scss',
})
export class UpdatePresent implements OnInit {

  constructor(private fb: FormBuilder, private presentService: PresentService, private donorService: DonorService, private router: Router, private route: ActivatedRoute){}

  addPresentForm!: FormGroup;
  donorsList: DonorModel[] = [];
  categoryList: CategoryModel[] = [];
  base64Image: string = '';
  presentId!: number;

  formFields = [
    { name: 'name', label: 'שם מתנה', type: 'text', errorMsg: 'שם מתנה הוא שדה חובה.' },
    { name: 'description', label: 'תיאור', type: 'text', errorMsg: 'תיאור הוא שדה חובה.' },
    { name: 'price', label: 'מחיר', type: 'number', errorMsg: 'מחיר חייב להיות חיובי.' },
    { name: 'pictureUrl', label: 'קישור לתמונה', type: 'text', errorMsg: 'קישור לתמונה הוא חובה.' },
    { name: 'purchasesAmount', label: 'כמות רכישות', type: 'number', errorMsg: 'כמות חייבת להיות 0 ומעלה.' },
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

    this.presentId = Number(this.route.snapshot.paramMap.get('id'));
    this.loadInitialData();
    if (this.presentId) {
      this.loadPresentDetails();
    }
  }

  loadInitialData() {
    this.presentService.getCategories().subscribe(data => this.categoryList = data);
    this.donorService.getDonors().subscribe(data => this.donorsList = data);
  }

  loadPresentDetails() {
    this.presentService.getPresents().pipe(take(1)).subscribe(presents => {
      const present = presents.find(p => p.id === this.presentId);
      if (present) {
        this.addPresentForm.patchValue({
          name: present.name,
          description: present.description,
          price: present.price,
          pictureUrl: present.pictureUrl,
          purchasesAmount: present.purchasesAmount,
          donorId: present.donorId,
          categoryId: present.categoryId
        });
        this.base64Image = present.pictureUrl || '';
      }
    });
  }

  onSubmit() {
    if (this.addPresentForm.valid) {
      const formValues = this.addPresentForm.value;
      const updatedData = {
        id: this.presentId,
        name: formValues.name,
        description: formValues.description,
        price: Number(formValues.price),
        pictureUrl: formValues.pictureUrl,
        purchasesAmount: Number(formValues.purchasesAmount),
        donorId: Number(formValues.donorId),
        categoryId: Number(formValues.categoryId)
      };

      this.presentService.updatePresent(updatedData as any).subscribe({
        next: () => {
          alert('המתנה עודכנה בהצלחה!');
          this.router.navigate(['api/present/getAll']);
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
        this.base64Image = reader.result as string;
        this.addPresentForm.patchValue({ pictureUrl: this.base64Image });
      };
      reader.readAsDataURL(file);
    }
  }

  goBack() {
    this.router.navigate(['api/present/getAll']);
  }
}