using System;
using System.Collections.Generic;
using CoreLayer.DTOs.Pagination;
using DomainLayer.Models.Newsletters;

namespace CoreLayer.DTOs.Admin
{
    public class NewslettersViewModel:BasePaging
    {
        public List<Newsletter> Newsletters { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Subject { get; set; }
        public int MemberCounts { get; set; }
    }
}