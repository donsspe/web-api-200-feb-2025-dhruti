﻿namespace IssueTracker.Api.Employee.Api;

public static class Extensions
{
    public static IEndpointRouteBuilder MapEmployees(this IEndpointRouteBuilder routes)
    {


        var group = routes.MapGroup("employee")
            .WithTags("Employees")
            .WithDescription("Employee Related Stuff")
            .RequireAuthorization(); // Check to make sure there is a trusted JWT on the Authorization header.

        group.MapPost("/software/{softwareId:guid}/problems",SubmittingAProblem.SubmitAsync);

        return routes;
    }
}
