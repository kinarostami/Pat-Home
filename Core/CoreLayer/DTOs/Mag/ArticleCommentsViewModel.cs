using System;
using System.Collections.Generic;
using CoreLayer.DTOs.Pagination;
using DomainLayer.Models.Articles;

namespace CoreLayer.DTOs.Mag
{
    public class ArticleCommentsViewModel : BasePaging
    {
        public List<ArticleComment> Comments { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }
}