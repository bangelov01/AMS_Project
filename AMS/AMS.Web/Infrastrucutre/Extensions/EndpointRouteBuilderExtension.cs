namespace AMS.Web.Infrastrucutre.Extensions
{
    public static class EndpointRouteBuilderExtension
    {
        public static void MapDefaultAreaRoute(this IEndpointRouteBuilder endpoints)
            => endpoints.MapControllerRoute(
                name: "Areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
    }
}
