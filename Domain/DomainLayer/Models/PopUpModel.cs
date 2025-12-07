namespace DomainLayer.Models
{
    public class PopUpModel:BaseEntity
    {
        public string Body { get; set; }
        public string LinkText { get; set; }
        public string LinkUrl { get; set; }
        public bool IsShow { get; set; }
    }
}