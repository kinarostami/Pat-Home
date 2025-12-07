namespace Common.Application.FileUtil
{
    public static class Directories
    {

        #region Seller
        public static string ShopImage = "wwwroot/shopAssets/images/Sellers/Avatars";
        public static string SellerImages = "wwwroot/shopAssets/images/Sellers/Uploads";
        public static string GetShopImage(string imageName) => $"{ShopImage.Replace("wwwroot", "")}/{imageName}";
        public static string GetSellerImage(string imageName) => $"{SellerImages.Replace("wwwroot", "")}/{imageName}";
        #endregion

        #region Shop
        //Shop
        public static string ProductImage = "wwwroot/shopAssets/images/Products";
        public static string BitMapProductImage = "wwwroot/shopAssets/images/Products/bitmap";

        public static string ProductGroup = "wwwroot/shopAssets/images/Groups";
        public static string BitMapProductGroup = "wwwroot/shopAssets/images/Groups/bitMap";
        public static string ProductGallery = "wwwroot/shopAssets/images/Products/Gallery";
        public static string ProductContent = "wwwroot/shopAssets/images/Products/content";
        public static string Sliders = "wwwroot/shopAssets/images/Sliders";
        public static string Banners = "wwwroot/shopAssets/images/Banners";
        public static string Brands = "wwwroot/shopAssets/images/Brands";

        public static string GetProductImage(string imageName) => $"{ProductImage.Replace("wwwroot", "")}/{imageName}";
        public static string GetBitMapProductImage(string imageName) => $"{BitMapProductImage.Replace("wwwroot", "")}/{imageName}";
        public static string GetProductGroup(string imageName) => $"{ProductGroup.Replace("wwwroot", "")}/{imageName}";
        public static string GetBitProductGroup(string imageName) => $"{BitMapProductGroup.Replace("wwwroot", "")}/{imageName}";
        public static string GetProductGallery(string imageName) => $"{ProductGallery.Replace("wwwroot", "")}/{imageName}";
        public static string GetSliderImage(string imageName) => $"{Sliders.Replace("wwwroot", "")}/{imageName}";
        public static string GetBannerImage(string imageName) => $"{Banners.Replace("wwwroot", "")}/{imageName}";
        public static string GetBrandImage(string imageName) => $"{Brands.Replace("wwwroot", "")}/{imageName}";

        #endregion

        #region Article
        //Article
        public static string Article = "wwwroot/magAssets/img/Articles";
        public static string ArticleContent = "wwwroot/magAssets/img/Articles/content";

        public static string GetArticleImage(string imageName) => $"{Article.Replace("wwwroot", "")}/{imageName}";
        public static string GetArticleContentImage(string imageName) => $"{ArticleContent.Replace("wwwroot", "")}/{imageName}";

        #endregion

        #region Users
        //Users

        public static string UserAvatar = "wwwroot/img/users/avatar";

        public static string GetUserAvatar(string imageName) => $"{UserAvatar.Replace("wwwroot", "")}/{imageName}";

        #endregion

        #region Newsletters

        public static string NewsLetterContent = "wwwroot/assets/newsLetters/content";
        public static string GetNewsLetterContent(string imageName) => $"{NewsLetterContent.Replace("wwwroot", "")}/{imageName}";
        #endregion
    }
}
