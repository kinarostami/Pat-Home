using System.Collections.Generic;
using System.Linq;
using DomainLayer.Models.Articles;

namespace CoreLayer.DTOs.Mag
{
    public class MagMainPageViewModel
    {
        public List<ArticleCard> LastArticles { get; set; }
        public List<ArticleCard> TopVisitArticles { get; set; }
        public List<ArticleCard> SpecialArticles { get; set; }
        public List<ArticleCard> PopularArticles { get; set; }
        public List<string> SpecialArticlesTitle { get; set; }
        public IQueryable<ArticleGroup> Categories { get; set; }
    }
}