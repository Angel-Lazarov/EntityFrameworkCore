namespace TestProject.Data
{
    public static class DataConstraints
    {
        //Boardgame
        public const int BoardgameNameMinLength = 10;
        public const int BoardgameNameMaxLength = 20;
        public const int BoardgameRatingMinValue = 1;
        public const int BoardgameRatingMaxValue = 10;
        public const int BoardgameYearPublishedMinValue = 2018;
        public const int BoardgameYearPublishedMaxValue = 2023;

        //Seller
        public const int SellerNameMinLength = 5;
        public const int SellerNameMaxLength = 20;
        public const int SellerAddresMinLength = 2;
        public const int SellerAddresMaxLength = 30;

        //Creator
        public const int CreatorFirstNameMinLengtht = 2;
        public const int CreatorFirstNameMaxLengtht = 7;
        public const int CreatorLastNameMinLengtht = 2;
        public const int CreatorLastNameMaxLengtht = 7;

    }
}
