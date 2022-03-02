namespace AMS.Data.Constants
{
    public class DataConstants
    {
        public class VehicleConstants
        {
            public const byte ConditionMaxLength = 20;
            public const byte MakeMaxLength = 30;
            public const byte ModelMaxLength = 50;
            public const short DescriptionMaxLength = 255;

        }

        public class AuctionConstants
        {
            public const short DescriptionMaxLength = 255;
        }

        public class AddressConstants
        {
            public const byte CountryNameMaxLength = 60;
            public const byte CityNameMaxLength = 90;
            public const byte AddressTextMaxLength = 200;
        }
    }
}
