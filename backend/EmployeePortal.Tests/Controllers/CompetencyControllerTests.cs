//using EmployeePortal.API.Controllers;
//using EmployeePortal.API.Data;
//using EmployeePortal.API.Data.Models;
//using EmployeePortal.API.DTOs;
//using Microsoft.AspNetCore.JsonPatch;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using Moq;
//using NUnit.Framework;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace EmployeePortal.Tests.Controllers
//{
//    [TestFixture]
//    public class CompetencyControllerTests
//    {
//        private EmployeeManagementContext _context;
//        private Mock<ILogger<CompetencyController>> _mockLogger;
//        private CompetencyController _controller;

//        [SetUp]
//        public void SetUp()
//        {
//            var options = new DbContextOptionsBuilder<EmployeeManagementContext>()
//                .UseInMemoryDatabase(databaseName: "EmployeeManagementTest")
//                .Options;

//            _context = new EmployeeManagementContext(options);
//            _mockLogger = new Mock<ILogger<CompetencyController>>();
//            _controller = new CompetencyController(_context, _mockLogger.Object);

//            ClearDatabase();
//            SeedDatabase();
//        }

//        [TearDown]
//        public void TearDown()
//        {
//            _context.Dispose();
//        }

//        private void ClearDatabase()
//        {
//            _context.Users.RemoveRange(_context.Users);
//            _context.Competencies.RemoveRange(_context.Competencies);
//            _context.SaveChanges();
//        }

//        private void SeedDatabase()
//        {
//            var user = new User
//            {
//                UserId = 1,
//                Name = "Test User",
//                Email = "testuser@example.com",
//                Role = "User",
//                UserName = "testuser",
//                PasswordHash = "hashedpassword",
//                BaseSalary = 50000m,
//                PerformanceMetrics = new List<PerformanceMetric>(),
//                Competencies = new List<Competency>(),
//                UserGroupMembers = new List<UserGroupMember>()
//            };

//            var manager = new User
//            {
//                UserId = 2,
//                Name = "Test Manager",
//                Email = "testmanager@example.com",
//                Role = "Manager",
//                UserName = "testmanager",
//                PasswordHash = "hashedpassword",
//                BaseSalary = 70000m,
//                PerformanceMetrics = new List<PerformanceMetric>(),
//                Competencies = new List<Competency>(),
//                UserGroupMembers = new List<UserGroupMember>()
//            };

//            var competency = new Competency
//            {
//                CompetencyId = 1,
//                UserId = 1,
//                TechnicalSkills = 5.0f,
//                Communication = 4.0f,
//                ProblemSolving = 4.5f,
//                Teamwork = 4.0f,
//                Leadership = 3.5f,
//                LastUpdated = DateTime.Now,
//                User = user,
//                Manager = manager
//            };

//            _context.Users.Add(user);
//            _context.Users.Add(manager);
//            _context.Competencies.Add(competency);
//            _context.SaveChanges();
//        }

//        [Test]
//        public async Task GetUserCompetencies_ReturnsOkResult_WithListOfCompetencies()
//        {
//            // Arrange
//            var userId = 1;

//            // Act
//            var result = await _controller.GetUserCompetencies(userId);

//            // Assert
//            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
//            var okResult = result.Result as OkObjectResult;
//            Assert.That(okResult.Value, Is.InstanceOf<List<CompetencyDto>>());
//            var competencyList = okResult.Value as List<CompetencyDto>;
//            Assert.That(competencyList.Count, Is.EqualTo(1));
//        }

//        [Test]
//        public async Task GetUsersUnderManager_ReturnsOkResult_WithListOfCompetencies()
//        {
//            // Arrange
//            var managerId = 2;

//            // Act
//            var result = await _controller.GetUsersUnderManager(managerId);

//            // Assert
//            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
//            var okResult = result.Result as OkObjectResult;
//            Assert.That(okResult.Value, Is.InstanceOf<List<Competency>>());
//            var competencyList = okResult.Value as List<Competency>;
//            Assert.That(competencyList.Count, Is.EqualTo(1));
//        }

//        [Test]
//        public async Task GetCompetency_ReturnsOkResult_WithCompetency()
//        {
//            // Arrange
//            var competencyId = 1; // Ensure this ID exists in the seeded data

//            // Act
//            var result = await _controller.GetCompetency(competencyId);

//            // Assert
//            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
//            var okResult = result.Result as OkObjectResult;
//            Assert.That(okResult.Value, Is.InstanceOf<Competency>());
//        }

//        [Test]
//        public async Task PostCompetency_ReturnsCreatedAtActionResult_WithCompetency()
//        {
//            // Arrange
//            var competencyDto = new CompetencyDto
//            {
//                UserId = 1,
//                TechnicalSkills = 4.0f,
//                Communication = 4.0f,
//                ProblemSolving = 4.0f,
//                Teamwork = 4.0f,
//                Leadership = 4.0f,
//                LastUpdated = DateTime.Now,
//                ManagerId = 2
//            };

//            // Act
//            var result = await _controller.PostCompetency(competencyDto);

//            // Assert
//            Assert.That(result.Result, Is.InstanceOf<CreatedAtActionResult>());
//            var createdResult = result.Result as CreatedAtActionResult;
//            Assert.That(createdResult.Value, Is.InstanceOf<Competency>());
//        }

//        [Test]
//        public async Task DeleteCompetency_ReturnsNoContent_WhenCompetencyDeleted()
//        {
//            // Arrange
//            var competencyId = 1;

//            // Act
//            var result = await _controller.DeleteCompetency(competencyId);

//            // Assert
//            Assert.That(result, Is.InstanceOf<NoContentResult>());
//        }

//        [Test]
//        public async Task UpdateCompetency_ReturnsNoContent_WhenCompetencyUpdated()
//        {
//            // Arrange
//            var competencyId = 1; // Ensure this ID exists in the seeded data
//            var competencyDto = new CompetencyDto
//            {
//                UserId = 2,
//                TechnicalSkills = 4.5f,
//                Communication = 4.0f,
//                ProblemSolving = 4.2f,
//                Teamwork = 4.3f,
//                Leadership = 4.1f,
//                LastUpdated = DateTime.UtcNow,
//                ManagerId = 3
//            };

//            // Act
//            var result = await _controller.PatchCompetency(competencyId, competencyDto);

//            // Assert
//            Assert.That(result, Is.InstanceOf<NoContentResult>());
//        }
//    }
//}

using EmployeePortal.API.Controllers;
using EmployeePortal.API.Data;
using EmployeePortal.API.Data.Models;
using EmployeePortal.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeePortal.Tests.Controllers
{
    [TestFixture]
    public class CompetencyControllerTests
    {
        private Mock<EmployeeManagementContext> _mockContext;
        private Mock<ILogger<CompetencyController>> _mockLogger;
        private CompetencyController _controller;
        private Mock<DbSet<User>> _mockUserSet;
        private Mock<DbSet<Competency>> _mockCompetencySet;

        [SetUp]
        public void SetUp()
        {
            _mockContext = new Mock<EmployeeManagementContext>();
            _mockLogger = new Mock<ILogger<CompetencyController>>();
            _mockUserSet = new Mock<DbSet<User>>();
            _mockCompetencySet = new Mock<DbSet<Competency>>();

            _mockContext.Setup(m => m.Users).Returns(_mockUserSet.Object);
            _mockContext.Setup(m => m.Competencies).Returns(_mockCompetencySet.Object);

            _controller = new CompetencyController(_mockContext.Object, _mockLogger.Object);

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            var users = new List<User>
    {
        new User
        {
            UserId = 1,
            Name = "Test User",
            Email = "testuser@example.com",
            Role = "User",
            UserName = "testuser",
            PasswordHash = "hashedpassword",
            BaseSalary = 50000m,
            PerformanceMetrics = new List<PerformanceMetric>(),
            Competencies = new List<Competency>(),
            UserGroupMembers = new List<UserGroupMember>()
        },
        new User
        {
            UserId = 2,
            Name = "Test Manager",
            Email = "testmanager@example.com",
            Role = "Manager",
            UserName = "testmanager",
            PasswordHash = "hashedpassword",
            BaseSalary = 70000m,
            PerformanceMetrics = new List<PerformanceMetric>(),
            Competencies = new List<Competency>(),
            UserGroupMembers = new List<UserGroupMember>()
        }
    }.AsQueryable();

            var competencies = new List<Competency>
    {
        new Competency
        {
            CompetencyId = 1,
            UserId = 1,
            TechnicalSkills = 5.0f,
            Communication = 4.0f,
            ProblemSolving = 4.5f,
            Teamwork = 4.0f,
            Leadership = 3.5f,
            LastUpdated = DateTime.Now,
            User = users.First(),
            Manager = users.Last()
        }
    }.AsQueryable();

            _mockUserSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            _mockUserSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            _mockUserSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            _mockUserSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            _mockCompetencySet.As<IQueryable<Competency>>().Setup(m => m.Provider).Returns(competencies.Provider);
            _mockCompetencySet.As<IQueryable<Competency>>().Setup(m => m.Expression).Returns(competencies.Expression);
            _mockCompetencySet.As<IQueryable<Competency>>().Setup(m => m.ElementType).Returns(competencies.ElementType);
            _mockCompetencySet.As<IQueryable<Competency>>().Setup(m => m.GetEnumerator()).Returns(competencies.GetEnumerator());

            _mockCompetencySet.Setup(m => m.FindAsync(It.IsAny<object[]>()))
                .ReturnsAsync((object[] ids) => competencies.FirstOrDefault(c => c.CompetencyId == (int)ids[0]));
        }

        [Test]
        public async Task GetCompetency_ReturnsOkResult_WithCompetency()
        {
            // Arrange
            var competencyId = 1; // Ensure this ID exists in the seeded data

            // Act
            var result = await _controller.GetCompetency(competencyId);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult.Value, Is.InstanceOf<Competency>());
        }

        [Test]
        public async Task DeleteCompetency_ReturnsNoContent_WhenCompetencyDeleted()
        {
            // Arrange
            var competencyId = 1;

            // Act
            var result = await _controller.DeleteCompetency(competencyId);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task UpdateCompetency_ReturnsNoContent_WhenCompetencyUpdated()
        {
            // Arrange
            var competencyId = 1; // Ensure this ID exists in the seeded data
            var competencyDto = new CompetencyDto
            {
                UserId = 2,
                TechnicalSkills = 4.5f,
                Communication = 4.0f,
                ProblemSolving = 4.2f,
                Teamwork = 4.3f,
                Leadership = 4.1f,
                LastUpdated = DateTime.UtcNow,
                ManagerId = 3
            };

            // Act
            var result = await _controller.PatchCompetency(competencyId, competencyDto);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }
    }
}