namespace AMS.Tests.Mocks
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    public static class ControllerContextMock
    {
        public static ControllerContext ControllerContextInstance(string claimName, string claimId)
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                   new Claim(ClaimTypes.Name, claimName),
                   new Claim(ClaimTypes.NameIdentifier, claimId)

            }, "mock"));

            return new ControllerContext() { HttpContext = new DefaultHttpContext() { User = user } };
        }
    }
}
