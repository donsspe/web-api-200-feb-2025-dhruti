namespace IssueTracker.Api.Employees.Domain;

// our "service" from last class.

public class Employee(EmployeeEntity entity, EmployeeRepository repository)
{
    public Guid Id { get; } = entity.Id;

    public ProblemSubmitted Process(SubmitProblem problemToSubmit)
    {
        var problem = new ProblemSubmitted(
            Guid.NewGuid(),
            problemToSubmit.SoftwareId, 
            Id, 
            problemToSubmit.Description, 
            DateTimeOffset.UtcNow);
        entity.Problems.Add(problem);
        return problem;
    }

    public async Task SaveAsync(CancellationToken token =default)
    {
        await repository.SaveAsync(entity, token);
    }
   
    
}

// commands are like commands - do this. 
public record SubmitProblem(Guid SoftwareId, string Description);

// events are past tense - this thing happened
public record ProblemSubmitted(Guid Id, Guid SoftwareId, Guid EmployeeId, string Description, DateTimeOffset Created);

