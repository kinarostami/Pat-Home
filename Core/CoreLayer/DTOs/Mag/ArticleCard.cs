using System;

namespace CoreLayer.DTOs.Mag
{
    public class ArticleCard
    {
        public long ArticleId { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public string ImageName { get; set; }
        public string CategoryName { get; set; }
        public string EnglishGroupTitle { get; set; }
        public string BuilderName { get; set; }
    }
}