import { ChangeDetectorRef, Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { PurchaseService } from '../../../Service/Purchase/purchase-service';
import { PurchaseModel } from '../../../Models/PurchaseModel';
import { CommonModule, DatePipe } from '@angular/common';

@Component({
  selector: 'app-purchase',
  standalone: true,
  imports: [CommonModule, DatePipe],
  templateUrl: './purchase.html',
  styleUrl: './purchase.scss',
})
export class Purchase implements OnChanges {

  @Input() presentId: number = 0;
  purchaseList: PurchaseModel[] = [];

  constructor(
    private purchaseService: PurchaseService,
    private cd: ChangeDetectorRef
  ) { }

  ngOnChanges(changes: SimpleChanges): void {
    const newId = changes['presentId']?.currentValue;
    if (newId && newId > 0) {
      this.loadPurchases(newId);
    }
  }

  loadPurchases(id: number): void {
    this.purchaseService.GetAllPurchasesOfPresent(id).subscribe({
      next: (purchases) => {
        console.log('נתונים שהתקבלו בחלון:', purchases);
        this.purchaseList = (purchases && purchases.length > 0) ? purchases : [];
        this.cd.detectChanges();
      },
      error: (error) => {
        console.error(error);
        this.purchaseList = [];
        this.cd.detectChanges();
      }
    });
  }
}