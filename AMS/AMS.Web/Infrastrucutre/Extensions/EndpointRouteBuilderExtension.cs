namespace AMS.Web.Infrastrucutre.Extensions
{
    public static class EndpointRouteBuilderExtension
    {
        public static void MapDefaultAreaRoute(this IEndpointRouteBuilder endpoints)
            => endpoints.MapControllerRoute(
                name: "Areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

        public static void MapCustomControllerRoutes(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapControllerRoute(
                 name: "Auction All",
                 pattern: "/Auctions/All/{currentPage?}",
                 defaults: new { controller = "Auctions", action = "All" });

            endpoints.MapControllerRoute(
                name: "Listings All",
                pattern: "/Listings/All/{id}/{currentPage?}",
                defaults: new { controller = "Listings", action = "All" });

            endpoints.MapControllerRoute(
                name: "Listings Details",
                pattern: "/Listings/Details/{auctionId}/{listingId}",
                defaults: new { controller = "Listings", action = "Details" });
        }
    }
}
