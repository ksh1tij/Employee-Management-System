using EmployeePortal.API.Data.Models;
using EmployeePortal.API.Data;
using System.Formats.Asn1;
using System.Globalization;
using CsvHelper;
using OfficeOpenXml;

namespace EmployeePortal.API.Services
{
    public class EmployeeService
    {
        private readonly EmployeeManagementContext _context;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(EmployeeManagementContext context, ILogger<EmployeeService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddEmployeesFromExcelAsync(Stream fileStream)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(fileStream))
            {
                var worksheet = package.Workbook.Worksheets.First();
                if (!ValidateExcelHeaders(worksheet))
                    throw new Exception("Invalid Excel headers");

                var rowCount = worksheet.Dimension.Rows;
                var users = new List<User>();

                for (int row = 2; row <= rowCount; row++)
                {
                    var name = worksheet.Cells[row, 1].Text;
                    var email = worksheet.Cells[row, 2].Text;
                    var phoneNumber = worksheet.Cells[row, 3].Text;
                    var address = worksheet.Cells[row, 4].Text;
                    var dateOfBirthString = worksheet.Cells[row, 5].Text;
                    var dateOfJoiningString = worksheet.Cells[row, 6].Text;
                    var designation = worksheet.Cells[row, 7].Text;
                    var role = worksheet.Cells[row, 8].Text;
                    var userName = worksheet.Cells[row, 9].Text;
                    var passwordHash = worksheet.Cells[row, 10].Text;
                    var baseSalaryString = worksheet.Cells[row, 11].Text;

                    var dateOfBirth = dateOfBirthString != null ? ParseDate(dateOfBirthString) : null;
                    var dateOfJoining = dateOfJoiningString != null ? ParseDate(dateOfJoiningString) : null;
                    var baseSalary = !string.IsNullOrEmpty(baseSalaryString) && Decimal.TryParse(baseSalaryString, out decimal parsedSalary) ? parsedSalary : 0;

                    if (_context.Users.Any(u => u.UserName == userName))
                    {
                        _logger.LogError("Username {UserName} already exists. Skipping this entry.", userName);
                        continue;
                    }

                    var user = new User
                    {
                        Name = name ?? throw new Exception("Name is required"),
                        Email = email ?? throw new Exception("Email is required"),
                        PhoneNumber = phoneNumber,
                        Address = address,
                        DateOfBirth = dateOfBirth ?? default(DateTime),
                        DateOfJoining = dateOfJoining ?? default(DateTime),
                        Designation = designation,
                        Role = role ?? throw new Exception("Role is required"),
                        UserName = userName ?? throw new Exception("UserName is required"),
                        PasswordHash = passwordHash ?? throw new Exception("PasswordHash is required"),
                        BaseSalary = baseSalary,
                        PerformanceMetrics = new List<PerformanceMetric>(),
                        Competencies = new List<Competency>(),
                        UserGroupMembers = new List<UserGroupMember>()
                    };
                    users.Add(user);
                }

                _context.Users.AddRange(users);
                await _context.SaveChangesAsync();
            }
        }

        private DateTime? ParseDate(string dateString)
        {
            string[] formats = { "dd/MM/yyyy", "MM/dd/yyyy", "yyyy-MM-dd", "yyyy-MM-dd HH:mm:ss.fffffff", "yyyy-MM-dd HH:mm:ss.fffffff" }; // Add more formats as needed
            try
            {
                if (DateTime.TryParseExact(dateString, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                {
                    return parsedDate;
                }
                else
                {
                    throw new FormatException($"Invalid date format: {dateString}");
                }
            }
            catch (FormatException ex)
            {
                // Log the exception message
                _logger.LogError(ex, "Error parsing date: {DateString}", dateString);
                return null;
            }
        }

        private bool ValidateExcelHeaders(ExcelWorksheet worksheet)
        {
            return worksheet.Cells[1, 1].Text == "Name" &&
                   worksheet.Cells[1, 2].Text == "Email" &&
                   worksheet.Cells[1, 3].Text == "PhoneNumber" &&
                   worksheet.Cells[1, 4].Text == "Address" &&
                   worksheet.Cells[1, 5].Text == "DateOfBirth" &&
                   worksheet.Cells[1, 6].Text == "DateOfJoining" &&
                   worksheet.Cells[1, 7].Text == "Designation" &&
                   worksheet.Cells[1, 8].Text == "Role" &&
                   worksheet.Cells[1, 9].Text == "UserName" &&
                   worksheet.Cells[1, 10].Text == "PasswordHash" &&
                   worksheet.Cells[1, 11].Text == "BaseSalary";
        }

        //public async Task AddEmployeesFromCsvAsync(Stream fileStream)
        //{
        //    using (var reader = new StreamReader(fileStream))
        //    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        //    {
        //        if (!ValidateCsvHeaders(csv))
        //            throw new Exception("Invalid CSV headers");

        //        var records = new List<User>();

        //        while (csv.Read())
        //        {
        //            var name = csv.GetField("Name");
        //            var email = csv.GetField("Email");
        //            var phoneNumber = csv.GetField("PhoneNumber");
        //            var address = csv.GetField("Address");
        //            var dateOfBirthString = csv.GetField("DateOfBirth");
        //            var dateOfJoiningString = csv.GetField("DateOfJoining");
        //            var designation = csv.GetField("Designation");
        //            var role = csv.GetField("Role");
        //            var userName = csv.GetField("UserName");
        //            var passwordHash = csv.GetField("PasswordHash");
        //            var baseSalaryString = csv.GetField("BaseSalary");

        //            var dateOfBirth = dateOfBirthString != null ? ParseDate(dateOfBirthString) : null;
        //            var dateOfJoining = dateOfJoiningString != null ? ParseDate(dateOfJoiningString) : null;
        //            var baseSalary = !string.IsNullOrEmpty(baseSalaryString) && Decimal.TryParse(baseSalaryString, out decimal parsedSalary) ? parsedSalary : 0;

        //            var user = new User
        //            {
        //                Name = name ?? throw new Exception("Name is required"),
        //                Email = email ?? throw new Exception("Email is required"),
        //                PhoneNumber = phoneNumber,
        //                Address = address,
        //                DateOfBirth = dateOfBirth ?? default(DateTime),
        //                DateOfJoining = dateOfJoining ?? default(DateTime),
        //                Designation = designation,
        //                Role = role ?? throw new Exception("Role is required"),
        //                UserName = userName ?? throw new Exception("UserName is required"),
        //                PasswordHash = passwordHash ?? throw new Exception("PasswordHash is required"),
        //                BaseSalary = baseSalary,
        //                PerformanceMetrics = new List<PerformanceMetric>(),
        //                Competencies = new List<Competency>(),
        //                UserGroupMembers = new List<UserGroupMember>()
        //            };
        //            records.Add(user);
        //        }

        //        _context.Users.AddRange(records);
        //        await _context.SaveChangesAsync();
        //    }
        //}

        //private bool ValidateCsvHeaders(CsvReader csv)
        //{
        //    var headers = csv.Context.Reader?.HeaderRecord;
        //    if (headers == null)
        //    {
        //        throw new Exception("CSV file does not contain headers.");
        //    }
        //    return headers.Contains("Name") && headers.Contains("Email") && headers.Contains("PhoneNumber") &&
        //           headers.Contains("Address") && headers.Contains("DateOfBirth") && headers.Contains("DateOfJoining") &&
        //           headers.Contains("Designation") && headers.Contains("Role") && headers.Contains("UserName") &&
        //           headers.Contains("PasswordHash") && headers.Contains("BaseSalary");
        //}
    }
}
