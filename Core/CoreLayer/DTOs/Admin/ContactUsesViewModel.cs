using System;
using System.Collections.Generic;
using CoreLayer.DTOs.Pagination;
using DomainLayer.Models;

namespace CoreLayer.DTOs.Admin
{
    public class ContactUsesViewModel:BasePaging
    {
        public List<ContactUs> ContactUses { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }
}