namespace AMS.Services.Infrastructure
{
    using AutoMapper;

    using AMS.Data.Models;

    using AMS.Services.Models;
    using AMS.Services.Models.Auctions;
    using AMS.Services.Models.Auctions.Base;
    using AMS.Services.Models.Listings;
    using AMS.Services.Models.Listings.Base;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Users
            this.CreateMap<User, UsersServiceModel>();

            //Watchlists
            this.CreateMap<Watchlist, SearchListingsServiceModel>()
                    .ForMember(dest => dest.AuctionId,opt => opt.MapFrom(src => src.Vehicle.AuctionId))
                    .ForMember(dest => dest.AuctionNumber,opt => opt.MapFrom(src => src.Vehicle.Auction.Number))
                    .ForMember(dest => dest.Id,opt => opt.MapFrom(src => src.Vehicle.Id))
                    .ForMember(dest => dest.Condition,opt => opt.MapFrom(src => src.Vehicle.Condition.Name))
                    .ForMember(dest => dest.Make,opt => opt.MapFrom(src => src.Vehicle.Model.Make.Name))
                    .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Vehicle.Model.Name))
                    .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Vehicle.ImageUrl))
                    .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Vehicle.Year));


            // Auctions
            this.CreateMap<Auction, AllAuctionsServiceModel>()
                    .ForMember(dest => dest.City,opt => opt.MapFrom(src => src.Address.City))
                    .ForMember(dest => dest.Country,opt => opt.MapFrom(src => src.Address.Country))
                    .ForMember(dest => dest.ListingsCount, opt => opt.MapFrom(src => src.Vehicles.Count(v => v.IsApproved == true)));

            this.CreateMap<Auction, AuctionServiceModel>()
                    .ForMember(dest => dest.City,opt => opt.MapFrom(src => src.Address.City));     
            
            this.CreateMap<Auction, AdminEditServiceModel>()
                    .ForMember(dest => dest.Country,opt => opt.MapFrom(src => src.Address.Country))
                    .ForMember(dest => dest.City,opt => opt.MapFrom(src => src.Address.City))
                    .ForMember(dest => dest.AddressText,opt => opt.MapFrom(src => src.Address.AddressText));

            //Listings
            this.CreateMap<Vehicle, ListingsServiceModel>()
                    .ForMember(dest => dest.Condition,opt => opt.MapFrom(src => src.Condition.Name))
                    .ForMember(dest => dest.Make,opt => opt.MapFrom(src => src.Model.Make.Name))
                    .ForMember(dest => dest.Model,opt => opt.MapFrom(src => src.Model.Name));

            this.CreateMap<Vehicle, ListingDetailsServiceModel>()
                    .ForMember(dest => dest.Type,opt => opt.MapFrom(src => src.Model.VehicleType.Name))
                    .ForMember(dest => dest.Condition,opt => opt.MapFrom(src => src.Condition.Name))
                    .ForMember(dest => dest.Make,opt => opt.MapFrom(src => src.Model.Make.Name))
                    .ForMember(dest => dest.Model,opt => opt.MapFrom(src => src.Model.Name));


            this.CreateMap<Vehicle, AdminListingsServiceModel>()
                    .ForMember(dest => dest.CreatorName,opt => opt.MapFrom(src => src.User.UserName))
                    .ForMember(dest => dest.Make,opt => opt.MapFrom(src => src.Model.Make.Name))
                    .ForMember(dest => dest.Model,opt => opt.MapFrom(src => src.Model.Name))
                    .ForMember(dest => dest.IsUserSuspended,opt => opt.MapFrom(src => src.User.IsSuspended));

            this.CreateMap<Vehicle, SearchListingsServiceModel>()
                    .ForMember(dest => dest.Make,opt => opt.MapFrom(src => src.Model.Make.Name))
                    .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Model.Name))
                    .ForMember(dest => dest.Condition,opt => opt.MapFrom(src => src.Condition.Name));

            this.CreateMap<Vehicle, MyListingsServiceModel>()
                    .ForMember(dest => dest.Make, opt => opt.MapFrom(src => src.Model.Make.Name))
                    .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Model.Name))
                    .ForMember(dest => dest.Condition, opt => opt.MapFrom(src => src.Condition.Name))
                    .ForMember(dest => dest.Start, opt => opt.MapFrom(src => src.Auction.Start))
                    .ForMember(dest => dest.End, opt => opt.MapFrom(src => src.Auction.End))
                    .ForMember(dest => dest.HighestBid, opt => opt.MapFrom(src => src.Bids.OrderByDescending(b => b.Amount).Take(1).Select(b => b.Amount).FirstOrDefault()));
        }
    }
}
