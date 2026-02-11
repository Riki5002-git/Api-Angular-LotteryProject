import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdatePresent } from './update-present';

describe('UpdatePresent', () => {
  let component: UpdatePresent;
  let fixture: ComponentFixture<UpdatePresent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UpdatePresent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UpdatePresent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
