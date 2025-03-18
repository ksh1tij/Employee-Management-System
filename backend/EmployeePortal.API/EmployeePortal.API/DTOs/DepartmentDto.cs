namespace EmployeePortal.API.DTOs
{
    public class DepartmentDto
    {
        public int DepartmentId { get; set; }
        public required string DepartmentName { get; set; }
        public int ManagerId { get; set; }
    }
}
