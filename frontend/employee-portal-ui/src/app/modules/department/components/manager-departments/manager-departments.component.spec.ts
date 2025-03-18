import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManagerDepartmentsComponent } from './manager-departments.component';

describe('ManagerDepartmentsComponent', () => {
  let component: ManagerDepartmentsComponent;
  let fixture: ComponentFixture<ManagerDepartmentsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ManagerDepartmentsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManagerDepartmentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
