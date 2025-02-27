using System.Security.Claims;
using Marten;

namespace IssueTracker.Api.Middleware;

public class AuthenticatedUserToEmployeeMiddleware(IDocumentSession session) : IEndpointFilter
{


    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {

        var sub = context?.HttpContext.User.FindFirstValue("sub");
        if (sub is not null)
        {
            // this is going through EVERY EVENT in the system (inline) and coming up with this result.
            var employee = await session.Query<Employee>().Where(u => u.Sub == sub).SingleOrDefaultAsync();

            if (employee is null)
            {
                var idForNewEmployee = Guid.NewGuid();
                session.Events.StartStream(idForNewEmployee, new EmployeeCreated(idForNewEmployee, sub));
            }
            else
            {
                session.Events.Append(employee.Id, new EmployeeHitApi(employee.Id));
            }

        }
        await session.SaveChangesAsync();

        return await next(context!);
    }
}

public record Employee
{
    public Guid Id { get; set; }
    public string Sub { get; set; } = string.Empty;

    public DateTimeOffset LastApiUsage { get; set; }

    public DateTimeOffset EmployeeCreated { get; set; }

    public static Employee Create(EmployeeCreated @event)
    {
        return new Employee
        {
            Id = @event.Id,
            Sub = @event.Sub,
            EmployeeCreated = DateTimeOffset.Now,
            LastApiUsage = DateTimeOffset.Now,
        };
    }

    public static Employee Apply(EmployeeHitApi _, Employee user)
    {
        return user with { LastApiUsage = DateTimeOffset.UtcNow };
    } 
}

public record EmployeeCreated(Guid Id, string Sub);

public  record EmployeeHitApi(Guid Id);