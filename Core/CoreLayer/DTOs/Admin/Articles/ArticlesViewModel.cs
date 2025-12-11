using System.Collections.Generic;
using CoreLayer.DTOs.Pagination;
using DomainLayer.Models.Articles;

namespace CoreLayer.DTOs.Admin.Articles
{
    public class ArticlesViewModel:BasePaging
    {
        public List<Article> Articles { get; set; }
        public long Id { get; set; } = 0;
        public string Title { get; set; }
        public string ShortLink { get; set; }
        public string SearchType { get; set; }
    }
}