import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManagedUsersComponent } from './managed-users.component';

describe('ManagedUsersComponent', () => {
  let component: ManagedUsersComponent;
  let fixture: ComponentFixture<ManagedUsersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ManagedUsersComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManagedUsersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
