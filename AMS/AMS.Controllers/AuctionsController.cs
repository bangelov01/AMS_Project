namespace AMS.Controllers
{
    using AMS.Controllers.Models;
    using AMS.Services.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static AMS.Services.Constants.ServicesConstants;

    public class AuctionsController : Controller
    {
        private readonly IAuctionService auctionService;

        public AuctionsController(IAuctionService auctionService)
        {
            this.auctionService = auctionService;
        }
    }
}
