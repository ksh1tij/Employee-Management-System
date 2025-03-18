using System;
using EmployeePortal.API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeePortal.API.Data
{
    public class DataInitializer
    {
        public static void Initialize(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<EmployeeManagementContext>();

                if (context == null)
                {
                    throw new Exception("EmployeeManagementContext is not registered in the service provider.");
                }

                // Look for any users.
                if (context.Users.Any() && context.Competencies.Any())
                {
                    return;   // Data was already seeded
                }

                var users = new List<User>
                {
                    new User
                    {
                        Name = "Alice Johnson",
                        Email = "alice.johnson@example.com",
                        PhoneNumber = "1234567890",
                        Address = "123 Main St",
                        DateOfBirth = new DateTime(1990, 5, 15),
                        DateOfJoining = new DateTime(2020, 1, 10),
                        Designation = "Software Engineer",
                        Role = "Employee",
                        UserName = "alice.johnson",
                        PasswordHash = "hashedpassword1",
                        PerformanceMetrics = new List<PerformanceMetric>(),
                        Competencies = new List<Competency>(),
                        UserGroupMembers = new List<UserGroupMember>()
                    },
                    new User
                    {
                        Name = "Bob Smith",
                        Email = "bob.smith@example.com",
                        PhoneNumber = "0987654321",
                        Address = "456 Elm St",
                        DateOfBirth = new DateTime(1985, 8, 20),
                        DateOfJoining = new DateTime(2019, 3, 15),
                        Designation = "Project Manager",
                        Role = "Manager",
                        UserName = "bob.smith",
                        PasswordHash = "hashedpassword2",
                        PerformanceMetrics = new List<PerformanceMetric>(),
                        Competencies = new List<Competency>(),
                        UserGroupMembers = new List<UserGroupMember>()
                    },
                    new User
                    {
                        Name = "Charlie Brown",
                        Email = "charlie.brown@example.com",
                        PhoneNumber = "1122334455",
                        Address = "789 Oak St",
                        DateOfBirth = new DateTime(1980, 12, 25),
                        DateOfJoining = new DateTime(2018, 7, 20),
                        Designation = "Administrator",
                        Role = "Admin",
                        UserName = "charlie.brown",
                        PasswordHash = "hashedpassword3",
                        PerformanceMetrics = new List<PerformanceMetric>(),
                        Competencies = new List<Competency>(),
                        UserGroupMembers = new List<UserGroupMember>()
                    }
                };

                context.Users.AddRange(users);
                context.SaveChanges();

                // Retrieve the UserId values after saving
                var alice = context.Users.Single(u => u.UserName == "alice.johnson");
                var bob = context.Users.Single(u => u.UserName == "bob.smith");
                var charlie = context.Users.Single(u => u.UserName == "charlie.brown");

                var departments = new List<Department>
                {
                    new Department
                    {
                        DepartmentName = "Human Resources",
                        ManagerId = bob.UserId,
                        Manager = bob,
                        UserDepartments = new List<UserDepartment>()
                    },
                    new Department
                    {
                        DepartmentName = "Engineering",
                        ManagerId = alice.UserId,
                        Manager = alice,
                        UserDepartments = new List<UserDepartment>()
                    }
                };

                context.Departments.AddRange(departments);
                context.SaveChanges();

                var userGroups = new List<UserGroup>
                {
                    new UserGroup
                    {
                        GroupName = "Development Team",
                        UserGroupMembers = new List<UserGroupMember>()
                    },
                    new UserGroup
                    {
                        GroupName = "Management Team",
                        UserGroupMembers = new List<UserGroupMember>()
                    }
                };

                context.UserGroups.AddRange(userGroups);
                context.SaveChanges();

                var userDepartments = new List<UserDepartment>
                {
                    new UserDepartment
                    {
                        UserId = alice.UserId,
                        DepartmentId = context.Departments.Single(d => d.DepartmentName == "Engineering").DepartmentId,
                        Department = context.Departments.Single(d => d.DepartmentName == "Engineering")
                    },
                    new UserDepartment
                    {
                        UserId = bob.UserId,
                        DepartmentId = context.Departments.Single(d => d.DepartmentName == "Human Resources").DepartmentId,
                        Department = context.Departments.Single(d => d.DepartmentName == "Human Resources")
                    },
                    new UserDepartment
                    {
                        UserId = charlie.UserId,
                        DepartmentId = context.Departments.Single(d => d.DepartmentName == "Human Resources").DepartmentId,
                        Department = context.Departments.Single(d => d.DepartmentName == "Human Resources")
                    }
                };

                context.UserDepartments.AddRange(userDepartments);
                context.SaveChanges();

                var userGroupMembers = new List<UserGroupMember>
                {
                    new UserGroupMember
                    {
                        UserId = alice.UserId,
                        GroupId = context.UserGroups.Single(g => g.GroupName == "Development Team").GroupId,
                        User = alice
                    },
                    new UserGroupMember
                    {
                        UserId = bob.UserId,
                        GroupId = context.UserGroups.Single(g => g.GroupName == "Management Team").GroupId,
                        User = bob
                    },
                    new UserGroupMember
                    {
                        UserId = charlie.UserId,
                        GroupId = context.UserGroups.Single(g => g.GroupName == "Management Team").GroupId,
                        User = charlie
                    }
                };

                context.UserGroupMembers.AddRange(userGroupMembers);
                context.SaveChanges();

                var performanceMetrics = new List<PerformanceMetric>
                {
                    new PerformanceMetric
                    {
                        UserId = alice.UserId,
                        TaskCompletionRate = 95.5f,
                        QualityOfWork = 90.0f,
                        AttendanceRate = 98.0f,
                        CustomerSatisfaction = 85.0f,
                        Efficiency = 92.0f,
                        Teamwork = 88.0f,
                        LastUpdated = DateTime.Now,
                        ManagerId = bob.UserId,
                        User = alice,
                        Manager = bob
                    },
                    new PerformanceMetric
                    {
                        UserId = bob.UserId,
                        TaskCompletionRate = 88.0f,
                        QualityOfWork = 85.0f,
                        AttendanceRate = 95.0f,
                        CustomerSatisfaction = 80.0f,
                        Efficiency = 90.0f,
                        Teamwork = 87.0f,
                        LastUpdated = DateTime.Now,
                        ManagerId = bob.UserId,
                        User = bob,
                        Manager = bob
                    }
                };

                context.PerformanceMetrics.AddRange(performanceMetrics);
                context.SaveChanges();

                var competencies = new List<Competency>
                {
                    new Competency
                    {
                        UserId = alice.UserId,
                        TechnicalSkills = 90.0f,
                        Communication = 85.0f,
                        ProblemSolving = 88.0f,
                        Teamwork = 87.0f,
                        Leadership = 80.0f,
                        LastUpdated = DateTime.Now,
                        ManagerId = bob.UserId,
                        User = alice,
                        Manager = bob
                    },
                    new Competency
                    {
                        UserId = bob.UserId,
                        TechnicalSkills = 85.0f,
                        Communication = 90.0f,
                        ProblemSolving = 87.0f,
                        Teamwork = 88.0f,
                        Leadership = 85.0f,
                        LastUpdated = DateTime.Now,
                        ManagerId = bob.UserId,
                        User = bob,
                        Manager = bob
                    }
                };

                context.Competencies.AddRange(competencies);
                context.SaveChanges();
            }
        }
    }
}