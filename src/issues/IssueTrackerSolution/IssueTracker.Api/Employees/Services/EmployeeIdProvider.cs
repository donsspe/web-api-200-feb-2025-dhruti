using System.Security.Claims;
using IssueTracker.Api.Middleware;
using Marten;

namespace IssueTracker.Api.Employees.Services;

public class EmployeeIdProvider(IDocumentSession session, IHttpContextAccessor context) : IProvideTheEmployeeId
{
    public async Task<Guid> GetEmployeeIdAsync(CancellationToken token = default)
    {
        var sub = context?.HttpContext?.User.FindFirstValue("sub") ??
                  throw new ChaosException("Used in an unauthenticated request or request without a subject claim");
        var employee = await session.Query<Employee>().Where(u => u.Sub == sub).SingleAsync(token);
        return employee.Id;
    }
}