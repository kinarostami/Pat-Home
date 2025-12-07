using System;

namespace DomainLayer.Models.Newsletters
{
    public class NewsletterMember:BaseEntity
    {
        public string Email { get; set; }
        public Guid MemberCode { get; set; }
    }
}