namespace EmployeePortal.API.DTOs
{
    public class PerformanceMetricDto
    {
        public int UserId { get; set; }
        public float TaskCompletionRate { get; set; }
        public float QualityOfWork { get; set; }
        public float AttendanceRate { get; set; }
        public float CustomerSatisfaction { get; set; }
        public float Efficiency { get; set; }
        public float Teamwork { get; set; }
        public DateTime LastUpdated { get; set; }
        public int ManagerId { get; set; }
    }
}
