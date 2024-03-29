﻿namespace AMS.Services.Models.Listings
{
    using AMS.Services.Models.Listings.Base;

    public class AdminListingsServiceModel : ListingsServiceModel
    {
        public string Description { get; init; }

        public string CreatorName { get; init; }

        public int BidsCount { get; init; }

        public bool IsUserSuspended { get; init; }
    }
}
