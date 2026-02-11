import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PersonService } from '../../Service/Person/PersonService';
import { PersonModel } from '../../Models/PersonModel';
import { take } from 'rxjs/operators';
import { ChangeDetectorRef } from '@angular/core';

@Component({
  selector: 'app-person',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './person.html',
  styleUrl: './Person.scss'
})
export class Person implements OnInit {
  people: PersonModel[] = [];
  isLoading: boolean = true;

  constructor(public personService: PersonService, private cd: ChangeDetectorRef) { }

  ngOnInit(): void {
    this.loadPeople();
  }

  loadPeople(): void {

    this.personService.getPeople().pipe(take(1)).subscribe({
      next: (data) => {
        this.people = data;
        this.isLoading = false;
        this.cd.detectChanges();
        console.log('הנתונים הגיעו בהצלחה:', data);
      },
      error: (err) => {
        this.isLoading = false;
        console.error('שגיאה בשליפת נתונים:', err);
      }
    });
  }
}