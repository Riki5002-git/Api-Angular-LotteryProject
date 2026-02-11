import { Component, OnInit, ChangeDetectorRef, Inject, Optional } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DonorService } from '../../../Service/Donor/donor-service';
import { PresentModel } from '../../../Models/PresentModel';

@Component({
  selector: 'app-donors-presents',
  standalone: true,
  imports: [CommonModule, MatDialogModule],
  templateUrl: './donors-presents.html',
  styleUrl: './donors-presents.scss',
})
export class DonorsPresents implements OnInit {
  presentsList: PresentModel[] = [];
  isLoading = true;

  constructor(
    private donorService: DonorService,
    private cdr: ChangeDetectorRef, @Optional() @Inject(MAT_DIALOG_DATA) public data: any, @Optional() @Inject(MatDialogRef) public dialogRef: MatDialogRef<DonorsPresents>) { }

  ngOnInit() {
    const donorId = this.data?.id;

    if (donorId) {
      this.getDonorPresents(donorId);
    } else {
      this.isLoading = false;
      console.warn('No donor ID provided to modal');
    }
  }

  getDonorPresents(donorId: number) {
    this.isLoading = true;
    this.donorService.getDonorsPresents(donorId).subscribe({
      next: (response: PresentModel[]) => {
        this.presentsList = response;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Error:', error);
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  close() {
    if (this.dialogRef) {
      this.dialogRef.close();
    }
  }
}