namespace AMS.Tests.Mocks
{
    using AMS.Services.Infrastructure;
    using AutoMapper;

    public class MapperMock
    {
        public static IMapper Instance
        {
            get
            {
                var mapperConfig = new MapperConfiguration(config =>
                {
                    config.AddProfile<MappingProfile>();
                });

                return new Mapper(mapperConfig);
            }
        }
    }
}
