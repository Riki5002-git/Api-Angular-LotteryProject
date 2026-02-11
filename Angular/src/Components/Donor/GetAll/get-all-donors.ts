import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { DonorModel } from '../../../Models/DonorModel';
import { DonorService } from '../../../Service/Donor/donor-service';
import { take } from 'rxjs/operators';
import { CommonModule } from '@angular/common';
import { PersonService } from '../../../Service/Person/PersonService';
import { Router } from '@angular/router';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { DonorsPresents } from '../donors-presents/donors-presents';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-get-all-donors',
  standalone: true,
  imports: [CommonModule, MatDialogModule, FormsModule],
  templateUrl: './get-all-donors.html',
  styleUrl: './get-all-donors.scss',
})
export class GetAllDonors implements OnInit {

  donors: DonorModel[] = [];
  isLoading: boolean = true;
  searchFullName: string = '';
  searchEmail: string = '';
  searchPresent: string = '';

  constructor(private donorService: DonorService, private cd: ChangeDetectorRef, public personService: PersonService, private router: Router, private dialog: MatDialog) { }

  ngOnInit(): void {
    this.loadDonors();
  }

  loadDonors(): void {
    this.donorService.getDonors().pipe(take(1)).subscribe({
      next: (data) => {
        this.donors = data;
        this.isLoading = false;
        this.cd.detectChanges();
      },
      error: (err) => {
        this.isLoading = false;
        console.error('שגיאה בשליפת נתונים:', err);
      }
    });
  }

  addDonor(): void {
    this.router.navigate(['/api/donor/add']);
  }

  deleteDonor(donorId: number): void {
    this.donorService.deleteDonor(donorId).pipe(take(1)).subscribe({
      next: () => {
        this.donors = this.donors.filter(donor => donor.id !== donorId);
        this.cd.detectChanges();
      },
      error: (err) => console.error('שגיאה במחיקת תורם:', err)
    });
  }

  updateDonor(donorId: number): void {
    this.router.navigate([`/api/donor/update/${donorId}`]);
  }

  viewPresents(donorId: number): void {
    this.dialog.open(DonorsPresents, {
      width: '800px',
      maxHeight: '90vh',
      direction: 'rtl',
      data: { id: donorId }
    });
  }

  filterDonorsByName(fullName: string): void {
    if (!fullName.trim()) {
      this.loadDonors();
      return;
    }
    this.isLoading = true;
    this.donorService.filterDonorsByName(fullName).pipe(take(1)).subscribe({
      next: (data: any) => {
        this.donors = data ? (Array.isArray(data) ? data : [data]) : [];
        this.isLoading = false;
        this.cd.detectChanges();
      },
      error: (err) => {
        this.donors = [];
        this.isLoading = false;
        this.cd.detectChanges();
      }
    });
  }

  filterDonorsByEmail(email: string): void {
    if (!email.trim()) {
      this.loadDonors();
      return;
    }

    this.isLoading = true;
    this.donorService.filterDonorsByEmail(email).pipe(take(1)).subscribe({
      next: (data: any) => {
        this.donors = data ? (Array.isArray(data) ? data : [data]) : [];
        this.isLoading = false;
        this.cd.detectChanges();
      },
      error: (err) => {
        console.error('שגיאה בפילטר אימייל:', err);
        this.donors = [];
        this.isLoading = false;
        this.cd.detectChanges();
      }
    });
  }

  filterDonorsByPresent(presentName: string): void {
    if (!presentName.trim()) {
      this.loadDonors();
      return;
    }

    this.isLoading = true;
    this.donorService.filterDonorsByPresent(presentName).pipe(take(1)).subscribe({
      next: (data: any) => {
        this.donors = data ? (Array.isArray(data) ? data : [data]) : [];
        this.isLoading = false;
        this.cd.detectChanges();
      },
      error: (err) => {
        console.error('שגיאה בפילטר מתנה:', err);
        this.donors = [];
        this.isLoading = false;
        this.cd.detectChanges();
      }
    });
  }

  clearFilters(): void {
    this.searchFullName = '';
    this.searchEmail = '';
    this.searchPresent = '';
    this.loadDonors();
    this.cd.detectChanges();
  }
}