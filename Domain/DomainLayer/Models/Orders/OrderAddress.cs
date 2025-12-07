namespace DomainLayer.Models.Orders
{
    public class OrderAddress:BaseEntity
    {
        public long OrderId { get; set; }
        public string PostalCode { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Shire { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public string Phone { get; set; }
        public string NationalCode { get; set; }

        public Order Order { get; set; }
    }
}