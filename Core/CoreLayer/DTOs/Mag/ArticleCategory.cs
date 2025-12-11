using System.Collections.Generic;
using CoreLayer.DTOs.Pagination;
using DomainLayer.Models.Articles;

namespace CoreLayer.DTOs.Mag
{
    public class ArticleCategory:BasePaging
    {
        public List<ArticleCard> Articles { get; set; }
        public ArticleGroup Category { get; set; }
        public List<ArticleGroup> ArticleGroups { get; set; }
        public string CategoryTitle { get; set; }
        public string Search { get; set; }
    }   
}