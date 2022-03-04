namespace AMS.Data.Constants
{
    public class DataConstants
    {
        public class ErrorMessages
        {
            public const string StringLengthValidationError = "{0} must be between {2} and {1} characters long!";
        }

        public class UserConstants
        {
            public const int UsernameMinLength = 5;
            public const int UsernameMaxLength = 30;
        }

        public class VehicleConstants
        {
            public const byte ConditionMaxLength = 20;
            public const byte MakeMaxLength = 30;
            public const byte TypeMaxLength = 30;
            public const byte ModelMaxLength = 50;
            public const short DescriptionMaxLength = 255;

        }

        public class AuctionConstants
        {
            public const short DescriptionMaxLength = 255;
            public const short DescriptionMinLength = 5;
        }

        public class AddressConstants
        {
            public const byte CountryNameMinLength = 4;
            public const byte CountryNameMaxLength = 60;

            public const byte CityNameMinLength = 1;
            public const byte CityNameMaxLength = 90;

            public const byte AddressTextMinLength = 20;
            public const byte AddressTextMaxLength = 200;
        }
    }
}
