import { Component, OnInit, ChangeDetectorRef } from '@angular/core'; // ייבוא ה-ChangeDetectorRef
import { CommonModule } from '@angular/common';
import { PurchaseService } from '../../../Service/Purchase/purchase-service';

@Component({
  selector: 'app-purchases-details',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './purchases-details.html',
  styleUrl: './purchases-details.scss',
})
export class PurchasesDetails implements OnInit {
  purchasesList: any[] = [];

  constructor(
    private purchaseService: PurchaseService,
    private cd: ChangeDetectorRef
  ) { }

  ngOnInit() {
    this.purchaseService.GetAllBuyersOfPresent().subscribe({
      next: (data) => {
        console.log('נתונים הגיעו לקומפוננטה:', data);
        this.purchasesList = data;
        this.cd.detectChanges();
      },
      error: (err) => {
        console.error('שגיאה בשליפת נתונים:', err);
      }
    });
  }
}