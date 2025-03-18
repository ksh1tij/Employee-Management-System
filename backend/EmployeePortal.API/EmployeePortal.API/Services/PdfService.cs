using EmployeePortal.API.Data.Models;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout;
using EmployeePortal.API.DTOs;

namespace EmployeePortal.API.Services
{
    public class PdfService
    {
        public byte[] GeneratePayslipPdf(Payslip payslip, string fileName)
        {
            using (var ms = new MemoryStream())
            {
                var writer = new PdfWriter(ms);
                var pdf = new PdfDocument(writer);
                var document = new Document(pdf);

                document.Add(new Paragraph($"Payslip for {payslip.User.Name}"));
                document.Add(new Paragraph($"Gross Pay: {payslip.GrossPay:C}"));
                document.Add(new Paragraph($"Deductions: {payslip.Deductions:C}"));
                document.Add(new Paragraph($"Net Pay: {payslip.NetPay:C}"));
                document.Add(new Paragraph($"Date: {payslip.Date:dd-MM-yyyy}"));

                document.Close();

                // Save the PDF to a file with the specified name
                File.WriteAllBytes(fileName, ms.ToArray());

                return ms.ToArray();
            }
        }
    }
}
