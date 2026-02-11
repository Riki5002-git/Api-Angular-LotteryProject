import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { BasketService } from '../../Service/Basket/basket-service';
import { PersonService } from '../../Service/Person/PersonService';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { PurchaseService } from '../../Service/Purchase/purchase-service';

@Component({
  selector: 'app-basket',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './basket.html',
  styleUrl: './basket.scss',
})
export class Basket implements OnInit {
  myGifts: any[] = [];

  constructor(
    private basketService: BasketService,
    public personService: PersonService,
    private cd: ChangeDetectorRef,
    private purchaseService: PurchaseService
  ) { }

  ngOnInit() {
    this.loadBasket();
    this.basketService.basketUpdated$.subscribe(() => {
      this.loadBasket();
    });
  }

  loadBasket() {
    const userId = this.personService.getUserId();
    if (!userId) return;

    this.basketService.getMyBasket(userId).subscribe({
      next: (response: any) => {
        if (response && response.presents) {
          this.myGifts = response.presents;
          console.log("Basket loaded with ID:", response.id);
        } else {
          this.myGifts = [];
        }
        this.cd.detectChanges();
      },
      error: (err) => {
        console.error("שגיאה בטעינת הסל:", err);
        this.myGifts = [];
      }
    });
  }

  addItemToBasket(present: any) {
    const userId = this.personService.getUserId();
    if (!userId || !present) return;

    this.basketService.addToBasket(userId, present.id).subscribe({
      next: () => {
        const itemInBasket = this.myGifts.find(g => g.presentId === present.id);
        if (itemInBasket) {
          itemInBasket.quantity++;
        }
        this.cd.detectChanges();
      }
    });
  }

  removeItemFromBasket(present: any) {
    const userId = this.personService.getUserId();
    if (!userId || !present) return;
    const itemInBasket = this.myGifts.find(g => g.presentId === present.id);
    if (!itemInBasket) return;
    if (itemInBasket.quantity > 1) {
      this.basketService.removeFromBasket(userId, present.id).subscribe({
        next: () => {
          itemInBasket.quantity--;
          this.cd.detectChanges();
        }
      });
    } else {
      this.removeItem(present);
    }
  }

  totalSum(): number {
    if (!this.myGifts) return 0;
    return this.myGifts.reduce((acc, item) => {
      const price = item.present?.price || 0;
      const qty = item.quantity || 1;
      return acc + (price * qty);
    }, 0);
  }

  removeItem(present: any) {
    if (confirm('להסיר את המוצר מהסל?')) {
      const userId = this.personService.getUserId();
      if (!userId || !present) return;

      this.basketService.clearItemCompletely(userId, present.id).subscribe({
        next: () => {
          this.myGifts = this.myGifts.filter(g => g.presentId !== present.id);
          this.cd.detectChanges();
        }
      });
    }
  }

  addPurchase() {
    const rawPersonId = this.personService.getUserId();
    const rawBasketId = this.basketService.getBasketId();
    console.log("IDs found:", { person: rawPersonId, basket: rawBasketId });

    if (!rawPersonId || !rawBasketId) {
      alert('שגיאה: חסרים נתוני משתמש או סל.');
      return;
    }

    const personId = Number(rawPersonId) as number;
    const basketId = Number(rawBasketId) as number;

    this.purchaseService.addPurchase(personId, basketId).subscribe({
      next: () => {
        alert('ההזמנה בוצעה בהצלחה!');
        localStorage.removeItem('basketId');
        this.myGifts = [];
        this.cd.detectChanges();
      },
      error: (err) => {
        console.error('שגיאת שרת ברכישה:', err);
        alert('לא הצלחנו לבצע את ההזמנה');
      }
    });
  }
}