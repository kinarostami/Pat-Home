namespace DomainLayer.Models.Articles;

public class ArticleComment : BaseEntity
{
    public long ArticleId { get; set; }
    public long UserId { get; set; }    
    public string Text { get; set; }

    public Article Article { get; set; }

    //[ForeignKey("UserId")]
    //public User User { get; set; }
}