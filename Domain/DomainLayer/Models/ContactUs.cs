using System;

namespace DomainLayer.Models
{
    public class ContactUs:BaseEntity
    {
        public string Subject { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }
        public string MessageBody { get; set; }
        public bool IsSeenAdmin { get; set; }
        public string Answer { get; set; }
    }
}