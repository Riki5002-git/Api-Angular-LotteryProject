import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DonorsPresents } from './donors-presents';

describe('DonorsPresents', () => {
  let component: DonorsPresents;
  let fixture: ComponentFixture<DonorsPresents>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DonorsPresents]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DonorsPresents);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
