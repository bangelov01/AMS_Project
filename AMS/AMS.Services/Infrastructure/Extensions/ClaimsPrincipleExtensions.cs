namespace AMS.Services.Infrastructure.Extensions
{
    using System.Security.Claims;

    using static AMS.Services.Constants.ServicesConstants;

    public static class ClaimsPrincipleExtensions
    {
        public static string Id(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public static bool IsAdmin(this ClaimsPrincipal user)
        {
            return user.IsInRole(AdministratorRoleName);
        }
    }
}
