import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { LotteryService } from '../../Service/Lottery/lottery-service';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { PresentService } from '../../Service/Present/present-service';
import { PresentModel } from '../../Models/PresentModel';

@Component({
  selector: 'app-lottery',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './lottery.html',
  styleUrl: './lottery.scss',
})
export class Lottery implements OnInit {

  constructor(
    private lotteryService: LotteryService,
    private route: ActivatedRoute,
    private presentService: PresentService,
    private cdr: ChangeDetectorRef
  ) { }

  message: string = '';
  isError: boolean = false;
  presents: PresentModel[] = [];

  ngOnInit(): void {
    this.loadPresents();
  }

  loadPresents(): void {
    this.presentService.getPresents().subscribe({
      next: (data) => {
        this.presents = [...data];
        console.log('נתונים נטענו:', this.presents);
        this.cdr.detectChanges();
      },
      error: (err) => {
        if (err.status === 500) {
          alert("לא ניתן לבצע הגרלה: כנראה שאין רוכשים למתנה זו");
        }
        console.error('שגיאה בטעינת מתנות:', err);
        this.isError = true;
        this.message = "שגיאה בטעינת הרשימה";
      }
    });
  }

  Lottery(pId: number): void {
    this.lotteryService.Lottery(pId).subscribe({
      next: (res) => {
        this.message = "ההגרלה בוצעה בהצלחה!";
        const index = this.presents.findIndex(p => p.id === pId);
        if (index !== -1) {
          this.presents[index] = {
            ...this.presents[index],
            winnerId: res.winnerId,
            winner: res.winner
          };
          this.presents = [...this.presents];
          this.cdr.detectChanges();
        }
      },
      error: (err) => {
        if (err.status === 500) {
          alert("לא ניתן לבצע הגרלה: כנראה שאין רוכשים למתנה זו");
        }
        this.isError = true;
        this.message = "שגיאה בשרת - בדקי אם ההגרלה בוצעה";
      }
    });
  }
}