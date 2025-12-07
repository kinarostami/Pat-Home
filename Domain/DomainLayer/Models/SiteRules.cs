using System;

namespace DomainLayer.Models
{
    public class SiteRules : BaseEntity
    {
        public string MainRule { get; set; }
        public DateTime LastModify { get; set; }
    }
}
