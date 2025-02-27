

using Alba;
using IssueTracker.Tests.Fixtures;

namespace IssueTracker.Tests.Employees.Api;

public class AuthTests
{
    [Theory]
    [InlineData($"/employee/software/{SeededSoftware.VisualStudioCode}/problems")]
    public async Task MustBeAuthenticatedToPostToTheseUrls(string resource)
    {
        var host = await AlbaHost.For<Program>();

        await host.Scenario(api =>
        {
            api.Post.Url(resource);
            api.StatusCodeShouldBe(401);
        });
    }
}

