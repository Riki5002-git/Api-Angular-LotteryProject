import { Component, OnInit } from '@angular/core';
import { PresentModel } from '../../../Models/PresentModel';
import { PresentService } from '../../../Service/Present/present-service';
import { take } from 'rxjs/operators';
import { ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CategoryModel } from '../../../Models/CategoryModel';
import { Router } from '@angular/router';
import { PersonService } from '../../../Service/Person/PersonService';
import { BasketService } from '../../../Service/Basket/basket-service';
import { Basket } from '../../Basket/basket';
import { PurchaseService } from '../../../Service/Purchase/purchase-service';
import { Purchase } from '../../purchase/purchasesCards/purchase';

@Component({
  selector: 'app-present',
  standalone: true,
  imports: [CommonModule, Basket, Purchase],
  templateUrl: './present.html',
  styleUrl: './present.scss',
})
export class Present implements OnInit {

  presents: PresentModel[] = [];
  isLoading: boolean = true;
  categoriesList: CategoryModel[] = [];
  presentCategories: PresentModel[] = [];
  flag: boolean = false;

  constructor(private presentService: PresentService, private cd: ChangeDetectorRef, private router: Router, public personService: PersonService, private basketService: BasketService, private purchaseService: PurchaseService) { }

  ngOnInit(): void {
    this.loadCategories();
    this.loadPresents();
  }

  getCategoryName(categoryId: number | undefined): string {
    if (!this.categoriesList || categoryId === undefined) {
      return 'טוען...';
    }
    const category = this.categoriesList.find(c => c.id === categoryId);
    return category ? category.name : 'ללא קטגוריה';
  }

  loadCategories(): void {
    this.presentService.getCategories().pipe(take(1)).subscribe({
      next: (data) => {
        this.categoriesList = data;
        this.cd.detectChanges();
      },
      error: (err) => console.error('שגיאה בשליפת קטגוריות:', err)
    });
  }

  loadPresents(): void {
    this.presentService.getPresents().pipe(take(1)).subscribe({
      next: (data) => {
        this.presents = [...data];
        this.isLoading = false;
        this.cd.detectChanges();
        console.log('הנתונים התעדכנו והמסך רוענן');
      },
      error: (err) => {
        this.isLoading = false;
        console.error('שגיאה בשליפת נתונים:', err);
      }
    });
  }

  deletePresent(presentId: number): void {
    if (confirm('האם את בטוחה שברצונך למחוק מתנה זו?')) {
      this.presentService.deletePresent(presentId).pipe(take(1)).subscribe({
        next: () => {
          this.presents = this.presents.filter(p => p.id !== presentId);
          this.cd.detectChanges();
        },
        error: (err) => {
          console.error('שגיאה במחיקת מתנה:', err);
          if (err.status === 500) {
            alert('לא ניתן למחוק את המתנה. ייתכן שהיא מקושרת לכרטיסים שכבר נרכשו.');
          } else {
            alert('אופס! אירעה שגיאה בלתי צפויה במחיקה. נסי שוב מאוחר יותר.');
          }
        }
      });
    }
  }

  updatePresent(presentId: number): void {
    this.router.navigate([`/api/present/update/${presentId}`]);
  }

  filterByCategory(categoryId: number): void {
    if (categoryId === 0) {
      this.flag = false;
    }
    else {
      this.flag = true;
      this.presentCategories = this.presents.filter(p => p.categoryId === categoryId);
    }
  }

  filterByPrice(val: any): void {
    const num = Number(val);
    if (!num) return;
    const listToSort = this.flag ? this.presentCategories : this.presents;
    listToSort.sort((a: PresentModel, b: PresentModel) => {
      const priceA = Number(a.price);
      const priceB = Number(b.price);
      return num === 1 ? priceA - priceB : priceB - priceA;
    });
    this.cd.detectChanges();
  }

  addToBasket(personId: number, presentId: number) {
    this.basketService.addToBasket(personId, presentId).subscribe({
      next: (response) => {
        console.log('נוסף לסל בהצלחה:', response);
        alert('המתנה נוספה לסל בהצלחה!');
      },
      error: (err) => {
        console.error('שגיאה בהוספה לסל:', err);
        alert('אופס! משהו השתבש בהוספה לסל.');
      }
    });
  }

  sortPresents(value: string): void {
    let listToSort = this.flag ? this.presentCategories : this.presents;
    if (!value) {
      this.loadPresents();
      return;
    }
    if (value === 'price') {
      listToSort.sort((a, b) => Number(b.price) - Number(a.price));
    }
    else if (value === 'purchase') {
      this.purchaseService.GetSortPresentsByPopular().subscribe({
        next: (data) => {
          if (this.flag) {
            this.presentCategories = data.filter((p: PresentModel) => p.categoryId === this.presentCategories[0]?.categoryId);
          } else {
            this.presents = data;
          }
          this.cd.detectChanges();
        }
      });
    }
    this.cd.detectChanges();
  }

  showPurchases: boolean = false;
  selectedPresentId: number = 0;

  getPurchases(presentId: number): void {
    this.selectedPresentId = presentId;
    this.showPurchases = true;
  }

  closePurchases(): void {
    this.showPurchases = false;
  }
}