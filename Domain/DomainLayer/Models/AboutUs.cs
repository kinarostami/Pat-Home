using System;

namespace DomainLayer.Models
{
    public class AboutUs:BaseEntity
    {
        public string Body { get; set; }
        public DateTime LastModify { get; set; }
    }
}