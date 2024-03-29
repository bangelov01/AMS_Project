﻿namespace AMS.Tests.Mocks
{
    using AutoMapper;

    using AMS.Services.Infrastructure;

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
