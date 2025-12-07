using System.ComponentModel.DataAnnotations.Schema;

namespace DomainLayer.Models.Articles;

public class ArticleGroup : BaseEntity
{
    public string GroupLink { get; set; }
    public string EnglishTitle { get; set; }
    public string Description { get; set; }
    public long? ParentId { get; set; }

    [ForeignKey("ParentId")]
    public ICollection<ArticleGroup> ArticleGroups { get; set; }
    public ICollection<Article> ArticlesMainGroups { get; set; }
    public ICollection<Article> ArticlesParentGroups { get; set; }
}