using System;
using System.Collections.Generic;

namespace DomainLayer.Models.FAQs
{
    //سوالات متداول
    public class Faq:BaseEntity
    {
        public string FaqTitle { get; set; }
        
        public ICollection<FaqDetail> Children { get; set; }
    }
}