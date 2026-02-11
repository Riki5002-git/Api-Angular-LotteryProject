import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PurchasesDetails } from './purchases-details';

describe('PurchasesDetails', () => {
  let component: PurchasesDetails;
  let fixture: ComponentFixture<PurchasesDetails>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PurchasesDetails]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PurchasesDetails);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
