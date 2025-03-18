using EmployeePortal.API.Data;
using EmployeePortal.API.Data.Models;
using EmployeePortal.API.DTOs;
using EmployeePortal.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeePortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfController : ControllerBase
    {
        private readonly PdfService _pdfService;
        private readonly PayrollService _payrollService;

        public PdfController(PdfService pdfService, PayrollService payrollService)
        {
            _pdfService = pdfService;
            _payrollService = payrollService;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GeneratePdf([FromBody] PdfRequest request)
        {
            if (request == null || request.PayslipDto.UserId <= 0)
            {
                return BadRequest("Invalid request.");
            }

            try
            {
                // Generate the payslip using PayrollService
                var payslip = await _payrollService.GeneratePayslipAsync(request.PayslipDto.UserId);

                // Create a dynamic file name
                var fileName = $"Payslip_{payslip.User.Name}_{payslip.Date:yyyyMMdd}.pdf";

                // Convert the payslip to PDF using PdfService with the dynamic file name
                var pdfBytes = _pdfService.GeneratePayslipPdf(new Payslip
                {
                    UserId = payslip.UserId,
                    GrossPay = payslip.GrossPay,
                    Deductions = payslip.Deductions,
                    NetPay = payslip.NetPay,
                    Date = payslip.Date,
                    User = payslip.User
                }, fileName);

                return File(pdfBytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

    public class PdfRequest
    {
        public required PayslipDto PayslipDto { get; set; }
    }
}
