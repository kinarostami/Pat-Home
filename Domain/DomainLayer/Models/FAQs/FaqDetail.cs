namespace DomainLayer.Models.FAQs
{
    public class FaqDetail:BaseEntity
    {
        public long FaqId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }


        public Faq Faq { get; set; }
    }
}