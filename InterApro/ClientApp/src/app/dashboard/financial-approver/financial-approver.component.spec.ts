import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FinancialApproverComponent } from './financial-approver.component';

describe('FinancialApproverComponent', () => {
  let component: FinancialApproverComponent;
  let fixture: ComponentFixture<FinancialApproverComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FinancialApproverComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FinancialApproverComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
