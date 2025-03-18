namespace EmployeePortal.API.DTOs
{
    public class CompetencyDto
    {
        public int UserId { get; set; }
        public float TechnicalSkills { get; set; }
        public float Communication { get; set; }
        public float ProblemSolving { get; set; }
        public float Teamwork { get; set; }
        public float Leadership { get; set; }
        public DateTime LastUpdated { get; set; }
        public int ManagerId { get; set; }
    }
}
