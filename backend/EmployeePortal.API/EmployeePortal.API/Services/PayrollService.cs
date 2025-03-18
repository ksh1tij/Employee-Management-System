using EmployeePortal.API.Data.Models;
using EmployeePortal.API.Data;

namespace EmployeePortal.API.Services
{
    public class PayrollService
    {
        private readonly EmployeeManagementContext _context;

        public PayrollService(EmployeeManagementContext context)
        {
            _context = context;
        }

        public async Task<Payslip> GeneratePayslipAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) throw new Exception("User not found");

            var grossPay = user.BaseSalary;
            var deductions = CalculateDeductions(user);
            var netPay = grossPay - deductions;

            var payslip = new Payslip
            {
                UserId = userId,
                GrossPay = grossPay,
                Deductions = deductions,
                NetPay = netPay,
                Date = DateTime.Now,
                User = user
            };

            _context.Payslips.Add(payslip);
            await _context.SaveChangesAsync();

            return payslip;
        }

        private decimal CalculateDeductions(User user)
        {
            decimal taxRate = 0.20m; // 20% tax rate
            decimal healthInsurance = 200m; // Fixed health insurance deduction
            decimal retirementContributionRate = 0.05m; // 5% retirement contribution

            decimal taxDeduction = user.BaseSalary * taxRate;
            decimal retirementContribution = user.BaseSalary * retirementContributionRate;

            decimal totalDeductions = taxDeduction + healthInsurance + retirementContribution;
            return totalDeductions;
        }
    }
}
