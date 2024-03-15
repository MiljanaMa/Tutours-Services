using System.Security.Claims;
using Explorer.API;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Explorer.BuildingBlocks.Tests;

public class BaseWebIntegrationTest<TTestFactory> : IClassFixture<TTestFactory>
    where TTestFactory : WebApplicationFactory<Program>
{
    public BaseWebIntegrationTest(TTestFactory factory)
    {
        Factory = factory;
    }

    protected TTestFactory Factory { get; }

    protected static ControllerContext BuildContext(string id)
    {
        return new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim("personId", id)
                }))
            }
        };
    }
}