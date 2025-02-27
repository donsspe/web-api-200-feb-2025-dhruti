namespace IssueTracker.Api.Employees.Services;

public interface IProvideTheEmployeeId
{
    public Task<Guid> GetEmployeeIdAsync(CancellationToken token = default);
}