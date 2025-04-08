import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CompetencyComponent } from './competency.component';
import { CompetencyService } from '../../services/competency.service';
import { UserService } from '../../../user/services/user.service';
import { UserDepartmentService } from '../../../department/services/user-department.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { of } from 'rxjs';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { NgxChartsModule } from '@swimlane/ngx-charts';

interface UserDto {
  userId: number;
  name: string;
  email: string;
  phoneNumber: string;
  address: string;
  dateOfBirth: Date; // Change this to Date
  role: string;
  dateOfJoining: Date; // Ensure this is also Date if needed
  designation: string;
  userName: string;
}

describe('CompetencyComponent', () => {
  let component: CompetencyComponent;
  let fixture: ComponentFixture<CompetencyComponent>;
  let mockCompetencyService: jasmine.SpyObj<CompetencyService>;
  let mockUserService: jasmine.SpyObj<UserService>;
  let mockUserDepartmentService: jasmine.SpyObj<UserDepartmentService>;
  let mockModalService: jasmine.SpyObj<NgbModal>;

  beforeEach(async () => {
    mockCompetencyService = jasmine.createSpyObj('CompetencyService', ['getCompetency', 'getUserCompetencies', 'postCompetency']);
    mockUserService = jasmine.createSpyObj('UserService', ['getUser']);
    mockUserDepartmentService = jasmine.createSpyObj('UserDepartmentService', ['getManagerDepartments']);
    mockModalService = jasmine.createSpyObj('NgbModal', ['open']);

    await TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule,
        FormsModule,
        CommonModule,
        MatTableModule,
        NgxChartsModule
      ],
      declarations: [CompetencyComponent],
      providers: [
        { provide: CompetencyService, useValue: mockCompetencyService },
        { provide: UserService, useValue: mockUserService },
        { provide: UserDepartmentService, useValue: mockUserDepartmentService },
        { provide: NgbModal, useValue: mockModalService }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(CompetencyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should fetch competency on init', () => {
    const competency = {
      userId: 1,
      technicalSkills: 5,
      communication: 4,
      problemSolving: 4.5,
      teamwork: 4,
      leadership: 3.5,
      lastUpdated: new Date().toISOString(),
      managerId: 2
    };
    mockCompetencyService.getCompetency.and.returnValue(of(competency));

    component.ngOnInit();

    expect(mockCompetencyService.getCompetency).toHaveBeenCalledWith(1);
    expect(component.competencies).toEqual([competency]);
    expect(component.chartData1).toEqual([
      { name: 'Communication', value: competency.communication },
      { name: 'Leadership', value: competency.leadership },
      { name: 'Problem Solving', value: competency.problemSolving },
      { name: 'Teamwork', value: competency.teamwork },
      { name: 'Technical Skills', value: competency.technicalSkills }
    ]);
  });

  it('should fetch users under manager on init', () => {
    const users: UserDto[] = [{
      userId: 1,
      name: 'Test User',
      email: 'test@example.com',
      phoneNumber: '1234567890',
      address: '123 Test St',
      dateOfBirth: new Date('1990-01-01'), // Use Date object
      role: 'User',
      dateOfJoining: new Date('2020-01-01'), // Use Date object
      designation: 'Developer',
      userName: 'testuser'
    }];
    mockUserDepartmentService.getManagerDepartments.and.returnValue(of(users));
    mockUserService.getUser.and.returnValue(of(users[0]));

    component.ngOnInit();

    expect(mockUserDepartmentService.getManagerDepartments).toHaveBeenCalledWith(1);
    expect(component.usersUnderManager).toEqual(users);
    expect(component.userNames[1]).toEqual('Test User');
  });

  it('should open modal', () => {
    component.openModal(1);
    expect(mockModalService.open).toHaveBeenCalledWith(component.userInfoModal);
  });

  it('should post competency', () => {
    const competency = {
      userId: 1,
      technicalSkills: 5,
      communication: 4,
      problemSolving: 4.5,
      teamwork: 4,
      leadership: 3.5,
      lastUpdated: new Date().toISOString(),
      managerId: 2
    };
    mockCompetencyService.postCompetency.and.returnValue(of(competency));

    component.newCompetency = competency;
    component.postCompetency();

    expect(mockCompetencyService.postCompetency).toHaveBeenCalledWith(competency);
    expect(mockModalService.open).toHaveBeenCalled();
  });
});