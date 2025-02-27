namespace IssueTracker.Api.Employees.Api;

public record EmployeeProblemReadModel
{
 
    public Guid Id { get; set; }

    public Guid EmployeeId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public Guid SoftwareId { get; set; }
    public DateTimeOffset Opened { get; set; }


    public static EmployeeProblemReadModel Create(EmployeeSubmittedAProblem problem)
    {
        return new EmployeeProblemReadModel
        {
            Id = problem.ProblemId,
            Description = problem.Description,
            EmployeeId = problem.EmployeeId,
            Opened = DateTimeOffset.UtcNow,
            SoftwareId = problem.SoftwareId,
            Status = "Submitted"
        };
    }
    
}