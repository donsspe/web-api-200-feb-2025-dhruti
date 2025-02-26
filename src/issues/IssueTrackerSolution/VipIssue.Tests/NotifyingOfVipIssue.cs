
using Alba;

namespace VipIssue.Tests;

public class NotifyingOfVipIssue
{
    [Fact]
    public async Task CanAddANotification()
    {
        var host = await AlbaHost.For<Program>();
        var requestBody = new VipIssueCreateModel("http://localhost:1339/problems/6bbe43c5-337a-4695-9875-a70985c8778a", "My stuff is broken again! Help!");
        var postResponse = await host.Scenario(api =>
        {
            api
            .Post
            .Json(requestBody)
            .ToUrl("/vips/notifications");

            api.StatusCodeShouldBe(201);
        });

        var postResponseBody = postResponse.ReadAsJson<VipIssueResponseModel>();
        var locationHeader = postResponse.Context.Response.Headers.Location.First();
        Assert.NotNull(locationHeader);
        Assert.StartsWith("/vip/notifications/", locationHeader);
      

        Assert.NotNull(postResponse);

        Assert.Equal(requestBody.Problem, postResponseBody.Problem);
        Assert.Equal(requestBody.Description, postResponseBody.Description);
        Assert.Equal("Pending", postResponseBody.Status);
        Assert.NotEqual(Guid.Empty, postResponseBody.Id);

        var getResponse = await host.Scenario(api =>
        {
            api.Get.Url(locationHeader);
        });

        var getResponseBody = getResponse.ReadAsJson<VipIssueResponseModel>();

        Assert.Equal(postResponseBody, getResponseBody);
    }
}
