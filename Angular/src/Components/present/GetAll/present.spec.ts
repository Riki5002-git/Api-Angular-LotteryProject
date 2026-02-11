import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Present } from '../GetAll/present';

describe('Present', () => {
  let component: Present;
  let fixture: ComponentFixture<Present>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Present]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Present);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
